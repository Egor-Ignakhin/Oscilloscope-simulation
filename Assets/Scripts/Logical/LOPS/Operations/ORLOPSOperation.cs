namespace OscilloscopeSimulation.Logical.LOPS.Operations
{
    internal class ORLOPSOperation : LogicalOperationsProcessingSystem
    {
        protected override bool OperateBehindLogicalValues()
        {
            foreach (ILogicalValue blv in behindLogicalHolders)
            {
                if (blv.GetLogicalValue())
                {
                    return true;
                }
            }
            return false;
        }
    }
}