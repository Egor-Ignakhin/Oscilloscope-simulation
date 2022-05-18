namespace OscilloscopeSimulation.Logical.LOPS.Operations
{
    internal class ANDLOPSOperation : LogicalOperationsProcessingSystem
    {
        protected override bool OperateBehindLogicalValues()
        {
            foreach (ILogicalValue blv in behindLogicalHolders)
            {
                if (!blv.GetLogicalValue())
                {
                    return false;
                }
            }
            return true;
        }
    }
}