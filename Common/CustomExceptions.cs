using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Common
{
    public class UserIsBlacklistedException : Exception
    {
        public UserIsBlacklistedException(string message) : base(message) { }
    }

    public class GarageIsClosedException : Exception
    {
        public GarageIsClosedException(string message) : base(message) { }
    }

    public class ParameterNotValidException : Exception
    {
        public ParameterNotValidException(string message) : base(message) { }
    }

    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message) { }
    }

    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string message) : base(message) { }
    }

    public class VehicleTypeNotFoundException : Exception
    {
        public VehicleTypeNotFoundException(string message) : base(message) { }
    }

    public class OptionalExtraNotFoundException : Exception
    {
        public OptionalExtraNotFoundException(string message) : base(message) { }
    }

    public class DrivingLicenseNotFoundException : Exception
    {
        public DrivingLicenseNotFoundException(string message) : base(message) { }
    }

    public class SupportingDocumentNotFoundException : Exception
    {
        public SupportingDocumentNotFoundException(string message) : base(message) { }
    }

    public class FuelTypeNotFoundException : Exception
    {
        public FuelTypeNotFoundException(string message) : base(message) { }
    }

    public class ConfigurationNotFoundException : Exception
    {
        public ConfigurationNotFoundException(string message) : base(message) { }
    }

    public class ExtensionRequestNotFoundException : Exception
    {
        public ExtensionRequestNotFoundException(string message) : base(message) { }
    }

    public class LicenseImportFileNotFound : Exception
    {
        public LicenseImportFileNotFound(string message) : base(message) { }
    }

    public class AbiImportFileNotFoundException : Exception
    {
        public AbiImportFileNotFoundException(string message) : base(message) { }
    }




}