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

    // Staged Evidence Reveal tuning constants.
    // How many Evidence/Witness cards are visible before the player
    // has taken any investigative action at all.
    private const int StartingEvidenceCount = 2;
    private const int StartingWitnessCount = 1;

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
        var evidence = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var witnesses = await _caseRepository.GetWitnessesForCaseAsync(caseId);

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

        // Staged Evidence Reveal: how many investigative actions has the
        // player taken so far, in total? Every connection attempt (right or
        // wrong) and every elimination counts as "the player is actively
        // investigating," which is what unlocks the next lead - correctness
        // isn't required here, same philosophy as the Suspect unlock above.
        var actionsTaken = playerConnections.Count + eliminatedIds.Count;

        var unlockedEvidenceIds = CalculateStagedUnlockIds(
            evidence.Select(e => e.Id).OrderBy(id => id),
            StartingEvidenceCount,
            actionsTaken);

        var unlockedWitnessIds = CalculateStagedUnlockIds(
            witnesses.Select(w => w.Id).OrderBy(id => id),
            StartingWitnessCount,
            actionsTaken);

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

            UnlockedSuspectIds = unlockedSuspectIds,
            UnlockedEvidenceIds = unlockedEvidenceIds,
            UnlockedWitnessIds = unlockedWitnessIds
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

        foreach (var conn in playerConnections)
        {
            if (conn.FromType == "Suspect" &&
                (conn.ToType == "Evidence" || conn.ToType == "Witness"))
            {
                unlocked.Add(conn.FromId);
            }

            if (conn.ToType == "Suspect" &&
                (conn.FromType == "Evidence" || conn.FromType == "Witness"))
            {
                unlocked.Add(conn.ToId);
            }
        }

        return unlocked;
    }

    /// <summary>
    /// Staged Evidence Reveal calculation.
    ///
    /// Given the full set of ordered IDs for a node type (Evidence or
    /// Witness, ordered ascending by Id - the intended narrative reveal
    /// order), a starting count that's always visible for free, and how
    /// many investigative actions the player has taken so far, returns
    /// exactly which IDs should currently be visible on the board.
    ///
    /// This is purely derived - nothing here is stored. Calling this twice
    /// with the same inputs always returns the same result, and a
    /// completed case (which already has plenty of recorded actions) will
    /// naturally have everything unlocked without any special-casing.
    /// </summary>
    private static HashSet<int> CalculateStagedUnlockIds(
        IEnumerable<int> orderedIds,
        int startingCount,
        int actionsTaken)
    {
        var idsInOrder = orderedIds.ToList();

        int unlockCount = Math.Min(
            idsInOrder.Count,
            startingCount + actionsTaken);

        // Guard: never go negative even if startingCount were misconfigured to 0
        // and actionsTaken is 0 - Take(0) is safe and returns an empty set.
        return idsInOrder.Take(Math.Max(0, unlockCount)).ToHashSet();
    }

    private static int CalculateConfidence(
        int correctConnections,
        int totalRequiredConnections,
        int correctEliminatedSuspects,
        int totalInnocentSuspects)
    {
        double connectionRatio = totalRequiredConnections == 0
            ? 0
            : Math.Min(1.0, (double)correctConnections / totalRequiredConnections);

        double eliminationRatio = totalInnocentSuspects == 0
            ? 0
            : Math.Min(1.0, (double)correctEliminatedSuspects / totalInnocentSuspects);

        double rawConfidence = (connectionRatio * ConnectionWeight) + (eliminationRatio * EliminationWeight);

        return Math.Clamp((int)Math.Round(rawConfidence), 0, 100);
    }
}