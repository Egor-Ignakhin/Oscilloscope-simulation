namespace OscilloscopeSimulation.Logical.LOPS.Operations
{
    internal sealed class NANDLOPSOperation : ANDLOPSOperation
    {
        protected override bool OperateBehindLogicalValues()
        {
            return !base.OperateBehindLogicalValues();
        }
    }
}