using OpenTabletDriver.Desktop.Reflection;
using OpenTabletDriver.Platform.Pointer;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Desktop.Binding
{
    public class BindingState
    {
        private readonly IBinding _binding;

        public BindingState(IPluginFactory pluginFactory, InputDevice device, IMouseButtonHandler mouseButtonHandler, PluginSettings settings)
        {
            _binding = pluginFactory.Construct<IBinding>(settings, device, mouseButtonHandler)!;
        }

        private bool _previousState;

        public void Invoke(IDeviceReport report, bool newState) {
            float pressure = 0;
            if (report is ITabletReport tabletReport) pressure = tabletReport.Pressure;
            newState = newState & pressure > 0;

            if (_binding is IStateBinding stateBinding) {
                if (newState && !_previousState)
                    stateBinding.Press(report);
                else if (!newState && _previousState)
                    stateBinding.Release(report);
            }

            _previousState = newState;
        }
    }
}
