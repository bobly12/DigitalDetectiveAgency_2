using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class InvestigationProgressService : IInvestigationProgressService
{
    private readonly IBoardRepository _boardRepository;
    private readonly ICaseRepository _caseRepository;

    private const int ConfidenceThreshold = 75;
    private const double ConnectionWeight = 70.0;
    private const double EliminationWeight = 30.0;

    public InvestigationProgressService(
        IBoardRepository boardRepository,
        ICaseRepository caseRepository)
    {
        _boardRepository = boardRepository;
        _caseRepository = caseRepository;
    }

    public async Task<InvestigationProgressSummary> GetInvestigationProgressAsync(
        int caseId,
        string userId)
    {
        // Fetch everything once
        var playerConnections = await _boardRepository.GetConnectionsAsync(caseId, userId);
        var answerKey = await _boardRepository.GetAnswerKeyAsync(caseId);
        var eliminatedIds = await _boardRepository.GetEliminatedSuspectIdsAsync(caseId, userId);
        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);

        var correctConnections = playerConnections
            .Count(pc => IsValidConnection(pc, answerKey));

        var totalRequiredConnections = answerKey.Count;

        var totalInnocentSuspects = suspects.Count(s => !s.IsGuilty);

        var correctEliminatedSuspects = eliminatedIds.Count(id =>
            suspects.Any(s =>
                s.Id == id &&
                !s.IsGuilty));

        // NOTE: unlock no longer depends on the answer key — see method below.
        var unlockedSuspectIds = CalculateUnlockedSuspectIds(playerConnections);

        var confidence = CalculateConfidence(
            correctConnections,
            totalRequiredConnections,
            correctEliminatedSuspects,
            totalInnocentSuspects);

        return new InvestigationProgressSummary
        {
            Confidence = confidence,
            CanAccuse = confidence >= ConfidenceThreshold,

            PlayerConnections = playerConnections.Count,
            CorrectConnections = correctConnections,
            TotalRequiredConnections = totalRequiredConnections,

            CorrectEliminatedSuspects = correctEliminatedSuspects,
            TotalInnocentSuspects = totalInnocentSuspects,

            UnlockedSuspectIds = unlockedSuspectIds
        };
    }

    /// <summary>
    /// NOTE:
    /// Replace this implementation with the shared validation helper
    /// if your project already has one.
    /// This should be the ONLY definition of a valid connection.
    /// Still used for CorrectConnections / Confidence — just no longer
    /// used to gate the Motive/Alibi unlock (see below).
    /// </summary>
    private static bool IsValidConnection(
        ClueConnection playerConnection,
        List<CaseConnection> answerKey)
    {
        return answerKey.Any(correct =>

            (correct.FromType == playerConnection.FromType &&
             correct.FromId == playerConnection.FromId &&
             correct.ToType == playerConnection.ToType &&
             correct.ToId == playerConnection.ToId)

            ||

            (correct.FromType == playerConnection.ToType &&
             correct.FromId == playerConnection.ToId &&
             correct.ToType == playerConnection.FromType &&
             correct.ToId == playerConnection.FromId)

        );
    }

    /// <summary>
    /// A suspect's Motive/Alibi unlock as soon as the player connects them to
    /// any Evidence or Witness card — regardless of whether that connection
    /// turns out to be correct. This rewards actively investigating a lead,
    /// not just already knowing the right answer.
    ///
    /// Suspect-to-Suspect connections do NOT unlock anything, since that
    /// isn't "linking a clue" to them.
    ///
    /// Correctness is intentionally irrelevant here: a wrong guess still
    /// unlocks the suspect's file (so the player isn't punished for taking
    /// a shot and can course-correct), but it still contributes nothing to
    /// CorrectConnections/Confidence above, which remain strictly validated
    /// against the answer key. Unlocking and scoring are separate concerns.
    /// </summary>
    private static HashSet<int> CalculateUnlockedSuspectIds(
        IEnumerable<ClueConnection> playerConnections)
    {
        var unlocked = new HashSet<int>();

        foreach (var connection in playerConnections)
        {
            bool fromIsSuspect = connection.FromType == "Suspect";
            bool toIsSuspect = connection.ToType == "Suspect";

            bool fromIsClue = connection.FromType == "Evidence" || connection.FromType == "Witness";
            bool toIsClue = connection.ToType == "Evidence" || connection.ToType == "Witness";

            if (fromIsSuspect && toIsClue)
                unlocked.Add(connection.FromId);

            if (toIsSuspect && fromIsClue)
                unlocked.Add(connection.ToId);
        }

        return unlocked;
    }

    private static int CalculateConfidence(
        int correctConnections,
        int totalRequiredConnections,
        int correctEliminatedSuspects,
        int totalInnocentSuspects)
    {
        var connectionRatio =
            totalRequiredConnections == 0
                ? 0
                : Math.Min(
                    1.0,
                    (double)correctConnections / totalRequiredConnections);

        var eliminationRatio =
            totalInnocentSuspects == 0
                ? 0
                : Math.Min(
                    1.0,
                    (double)correctEliminatedSuspects / totalInnocentSuspects);

        var confidence =
            (connectionRatio * ConnectionWeight) +
            (eliminationRatio * EliminationWeight);

        return Math.Clamp(
            (int)Math.Round(confidence),
            0,
            100);
    }
}
