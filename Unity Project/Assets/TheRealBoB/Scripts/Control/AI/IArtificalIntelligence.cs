using System;

public interface IArtificalIntelligence
{
	/// <summary>
	/// Perform this to get the turn plan from any AI
	/// </summary>
	/// <returns>The planning.</returns>
	TurnPlan DoPlanning();
}

