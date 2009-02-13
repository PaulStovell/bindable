namespace Bindable.Windows.AutoCorrection
{
    public class Correction
    {
        private readonly object _originalValue;
        private readonly object _correctedValue;

        public Correction(object originalValue, object correctedValue)
        {
            _originalValue = originalValue;
            _correctedValue = correctedValue;
        }

        public object CorrectedValue
        {
            get { return _correctedValue; }
        }

        public object OriginalValue
        {
            get { return _originalValue; }
        }

        public static Correction DoNothing = new Correction(null, null);
    }
}