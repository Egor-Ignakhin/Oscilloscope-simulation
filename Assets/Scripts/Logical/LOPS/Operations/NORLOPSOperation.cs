namespace OscilloscopeSimulation.Logical.LOPS.Operations
{
    internal sealed class NORLOPSOperation : ORLOPSOperation
    {
        protected override bool OperateBehindLogicalValues()
        {
            return !base.OperateBehindLogicalValues();
        }
    }
}