using AutoMapper;
using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Credit;
using System;

namespace Likvido.CreditRisk.Domain.Mappers
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {   
            CreateMap<RegistrationDTO, Registration>()
                .ForMember(d => d.AdministratorName, c => c.MapFrom(s => s.AdministratorName))
                .ForMember(d => d.AdministratorPhone, c => c.MapFrom(s => s.AdministratorPhone))
                .ForMember(d => d.AdministratorReference, c => c.MapFrom(s => s.AdministratorReference))
                .ForMember(d => d.CreditorName, c => c.MapFrom(s => s.CreditorName))
                .ForMember(d => d.CreditorPhone, c => c.MapFrom(s => s.CreditorPhone))
                .ForMember(d => d.CreditorReference, c => c.MapFrom(s => s.CreditorReference))
                .ForMember(d => d.Date, c => c.MapFrom(s => s.Date))
                .ForMember(d => d.DeletionDate, c => c.MapFrom(s => s.DeletionDate))                
                .ForMember(d => d.DeptorId, c => c.MapFrom(s => s.DeptorId))
                .ForMember(d => d.Description, c => c.MapFrom(s => s.Description))
                .ForMember(d => d.Foundation, c => c.MapFrom(s => s.Foundation))
                .ForMember(d => d.InvoiceId, c => c.MapFrom(s => s.InvoiceId))
                .ForMember(d => d.RegistrationSystemId, c => c.MapFrom(s => s.RegistrationSystemId))
                .ForAllOtherMembers(c => c.Ignore());

            CreateMap<RegistrationPrivateDTO, RegistrationUser>();

            CreateMap<RegistrationCompanyDTO, RegistrationCompany>();                

            CreateMap<Registration, RegistrationDetailsDTO>();

            CreateMap<RegistrationDetailsDTO, Registration>();                

            CreateMap<RegistrationUser, RegistrationUserDTO>()
                .ForMember(d => d.RegistrationNumber, c => c.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.Address, c => c.MapFrom(s => s.Address))
                .ForMember(d => d.AddressTwo, c => c.MapFrom(s => s.AddressTwo))
                .ForMember(d => d.City, c => c.MapFrom(s => s.City))
                .ForMember(d => d.Country, c => c.MapFrom(s => s.Country))
                .ForMember(d => d.Email, c => c.MapFrom(s => s.Email))
                .ForMember(d => d.FirstName, c => c.MapFrom(s => s.FirstName))
                .ForMember(d => d.Id, c => c.MapFrom(s => s.Id))
                .ForMember(d => d.LastName, c => c.MapFrom(s => s.LastName))
                .ForMember(d => d.Phone, c => c.MapFrom(s => s.Phone))
                .ForMember(d => d.State, c => c.MapFrom(s => s.State))
                .ForMember(d => d.ZipCode, c => c.MapFrom(s => s.ZipCode))
                .ForMember(d => d.NumberOfRegistratins, c => c.MapFrom(s => s.Registrations.Count))
                .ForAllOtherMembers(c => c.Ignore());

            CreateMap<RegistrationUserDTO, PrivateData>()
                .ForMember(d => d.Address, c => c.MapFrom(s => s.Address))
                .ForMember(d => d.RegistrationNumber, c => c.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.AddressTwo, c => c.MapFrom(s => s.AddressTwo))
                .ForMember(d => d.City, c => c.MapFrom(s => s.City))
                .ForMember(d => d.Country, c => c.MapFrom(s => s.Country))
                .ForMember(d => d.Email, c => c.MapFrom(s => s.Email))
                .ForMember(d => d.FirstName, c => c.MapFrom(s => s.FirstName))                
                .ForMember(d => d.LastName, c => c.MapFrom(s => s.LastName))
                .ForMember(d => d.Phone, c => c.MapFrom(s => s.Phone))
                .ForMember(d => d.State, c => c.MapFrom(s => s.State))
                .ForMember(d => d.ZipCode, c => c.MapFrom(s => s.ZipCode))
                .ForAllMembers(c => c.MapAtRuntime());

            CreateMap<RegistrationUser, RegistrationPrivateDetailsDTO>();

            CreateMap<RegistrationCompany, RegistrationCompanyDetailsDTO>();

        }
    }
}
