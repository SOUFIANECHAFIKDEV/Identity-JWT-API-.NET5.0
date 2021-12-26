using System;
using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Vehicle_Detail Detail { get; set; }
        public Vehicle_Acquisition Acquisition { get; set; }
        public List<Vehicle_Document> Documents { get; set; }
        public List<Vehicle_Regular_Charge> RegularCharges { get; set; }
        public List<Vehicle_Maintenance> Maintenances { get; set; }
        public List<Accident> Accidents { get; set; }
    }
    public class Vehicle_Acquisition
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ContractNumber { get; set; }
        public int WrrantyInYears { get; set; }
        public double PricePreTax { get; set; }
        public double VAT { get; set; }
        public double PriceTaxIncl { get; set; }
    }
    public class Vehicle_Detail
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string NumberW { get; set; }
        public string NumberRegistration { get; set; }
        public DateTime RegistrationExpiryDate { get; set; }
        public int BrandId { get; set; } //Reference
        public int ModelId { get; set; } //Reference
        public int ColourId { get; set; } //Reference
        public int FuelTypeId { get; set; } //Reference
        public int KindId { get; set; } //Reference
        public int TransmissionId { get; set; } //Reference
        public int AcquisitionId { get; set; } //Reference
        public int ModelYear { get; set; }
        public int Kilometers { get; set; }
        public int BodyTypeId { get; set; } //Reference
        public string ChassisNumber { get; set; }
        public DateTime EntryIntoService { get; set; }
        public int Cylinders { get; set; }
        public int FiscalPower { get; set; }
        public R_Brand Brand { get; set; }
        public R_Model Model { get; set; }
        public R_Colour Colour { get; set; }
        public R_FuelType FuelType { get; set; }
        public R_Kind Kind { get; set; }
        public R_Transmission Transmission { get; set; }
        public R_Acquisition Acquisition { get; set; }
        public R_BodyType BodyType { get; set; }
    }
    public class Vehicle_Document
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modification { get; set; }
        public string Type { get; set; }
        public byte[] Blob { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    //---------------------------------------------

    public class Vehicle_Regular_Charge
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // foreign key
        public int Type { get; set; } //reference
        public List<Insurance> Insurances { get; set; }
        public List<PeriodicTechnicalInspection> PeriodicTechnicalInspections { get; set; }
        public List<YearlyTaxe> YearlyTaxes { get; set; }
        public List<Regular_Maintenance> RegularMaintenances { get; set; }
        public List<Vehicle_Charge_Document> Documents { get; set; }
    }

    public class Insurance
    {
        public int Id { get; set; }
        public int RegularChargeId { get; set; } // foreign key
        public int InsuranceCompanyId { get; set; } //reference
        public int IntermediaryAgencyId { get; set; } //reference
        public DateTime WarrantyPeriodStart { get; set; }
        public DateTime WarrantyPeriodEnd { get; set; }
        public double PricePreTax { get; set; }
        public double Tax { get; set; }
        public double PriceTaxIncl { get; set; }
        public int Branche { get; set; } //reference ??
        public int PassengersNumber { get; set; }
    }

    public class PeriodicTechnicalInspection
    {
        public int Id { get; set; }
        public int RegularChargeId { get; set; } // foreign key
        public DateTime InspectionDate { get; set; }
        public DateTime NextInspectionDate { get; set; }
        public double Price { get; set; }
        public string AgencyName { get; set; }
        public string AgencyAdress { get; set; }

    }

    public class YearlyTaxe {
        public int Id { get; set; }
        public int RegularChargeId { get; set; } // foreign key
        public int YearId { get; set; } // foreign key
        public DateTime PayementDate { get; set; }
        public double PricePreTax { get; set; }
        public double Tax { get; set; }
        public double ManagementFees { get; set; }
        public double PriceTaxIncl { get; set; }
    }
    
    public class Regular_Maintenance
    {
        public int Id { get; set; }
        public int RegularChargeId { get; set; } // foreign key
        public int Type { get; set; } // foreign key
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public Nullable<int> ValidatyByKM { get; set; }
        public Nullable<int> ValidatyByMonnthes { get; set; }
        public R_Regular_Maintenance R_RegularMaintenance { get; set; }
        public List<Vehicle_Charge_Document> Documents { get; set; }
    }

    public class R_Regular_Maintenance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasValidatyByKM { get; set; }
        public bool HasValidatyByMonnthes { get; set; }
        public List<Regular_Maintenance> RegularsMaintenances { get; set; }
    }

    public class Vehicle_Charge_Document
    {
        public int Id { get; set; }
        public int RegularChargeId { get; set; } // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modification { get; set; }
        public string Type { get; set; }
        public byte[] Blob { get; set; }
        public Regular_Maintenance RegularMaintenance { get; set; }
    }
    //---------------------------------------------

    public class Vehicle_Maintenance
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // foreign key
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public List<Vehicle_Maintenance_docs> Documents { get; set; }
    }

    public class Vehicle_Maintenance_docs
    {
        public int Id { get; set; }
        public int VehicleMaintenanceId { get; set; } // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modification { get; set; }
        public string Type { get; set; }
        public byte[] Blob { get; set; }
       
    }
    //---------------------------------------------

    public class Accident
    {
        public int Id { get; set; }
        public int VehicleId { get; set; } // foreign key
        public string Description { get; set; }
        public bool IsResponsible { get; set; }
        public DateTime AccidentDate { get; set; }
        public List<Accident_Document> Documents { get; set; }
        public List<Accident_Photos> Photos { get; set; }
        public List<Accident_Maintenance> Maintenances { get; set; }
    }

    public class Accident_Maintenance
    {
        public int Id { get; set; }
        public int AccidentId { get; set; } // foreign key
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime MaintenanceDate { get; set; }
    }

    public class Accident_Document
    {
        public int Id { get; set; }
        public int AccidentId { get; set; } // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modification { get; set; }
        public string Type { get; set; }
        public byte[] Blob { get; set; }
        public Accident Accident { get; set; }
    }

    public class Accident_Photos
    {
        public int Id { get; set; }
        public int AccidentId { get; set; } // foreign key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modification { get; set; }
        public string Type { get; set; }
        public byte[] Blob { get; set; }
        public Accident Accident { get; set; }
    }

    //--------------------------------------------
    public class R_Brand { }
    public class R_Model { }
    public class R_Colour { }
    public class R_FuelType { }
    public class R_Kind { }
    public class R_Transmission { }
    public class R_Acquisition { }
    public class R_BodyType { }
}
