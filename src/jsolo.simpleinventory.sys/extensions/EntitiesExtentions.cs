// using System.Collections.Generic;
// using System.Linq;

// using jsolo.simpleinventory.core.entities;
// using jsolo.simpleinventory.core.objects;
// using jsolo.simpleinventory.sys.models;



// namespace jsolo.simpleinventory.sys.extensions
// {
//     public static class EntitiesExtentions
//     {
//         public static CustomerViewModel ToViewModel(this Customer customer)
//         {
//             return new CustomerViewModel
//             {
//                 Id = customer.Id,
//                 AccountNo = customer.AccountNo,
//                 CompanyName = customer.BusinessName,
//                 Title = customer.PersonalName?.Title ?? string.Empty,
//                 LastName = customer.PersonalName?.Surname ?? string.Empty,
//                 FirstName = customer.PersonalName?.FirstName ?? string.Empty,
//                 Equipment = customer.Equipment?.Select(e => e.ToViewModel())?.ToList(),
//                 Address = customer.Address?.ToViewModel(),
//                 Feeder = customer.Feeder.ToViewModel(),
//                 Inspector = customer.Inspector?.ToViewModel() ?? null,
//                 IsEquipmentApproved = customer.IsApproved(),
//                 Comments = customer.Comments,
//                 CreatedOn = customer.CreatedOn,
//                 CreatedBy = customer.CreatorId?.ToString() ?? string.Empty,
//                 LastUpdatedOn = customer.LastModifiedOn,
//                 LastUpdatedBy = customer.LastModifierId?.ToString() ?? string.Empty
//             };
//         }


//         public static EquipmentViewModel ToViewModel(this Equipment equipment) => new EquipmentViewModel
//         {
//             Type = equipment.Type,
//             Amount = equipment.Amount,
//             Capacity = equipment.Capacity
//         };


//         public static InspectorViewModel ToViewModel(this Inspector inspector) => new InspectorViewModel
//         {
//             Id = inspector.Id,
//             EmployeeNo = inspector.TeamNo,
//             Title = inspector.Name?.Title ?? string.Empty,
//             LastName = inspector.Name?.Surname ?? string.Empty,
//             FirstName = inspector.Name?.FirstName ?? string.Empty,
//             CreatedOn = inspector.CreatedOn,
//             CreatedBy = inspector.CreatorId?.ToString() ?? string.Empty,
//             LastUpdatedOn = inspector.LastModifiedOn,
//             LastUpdatedBy = inspector.LastModifierId?.ToString() ?? string.Empty
//         };


//         public static FeederViewModel ToViewModel(this Feeder feeder) => new FeederViewModel
//         {
//             Id = feeder.Id,
//             Code = feeder.Code,
//             Name = feeder.Name
//         };


//         private static AddressViewModel ToViewModel(this Address address) => new AddressViewModel
//         {
//             Island = address.Island is null ? null : new IslandViewModel
//             {
//                 Id = address.Island.Id,
//                 Name = address.Island.Name
//             },
//             Parish = address.Parish is null ? null : new ParishViewModel
//             {
//                 Id = address.Parish.Id,
//                 Name = address.Parish.Name
//             },
//             Village = address.Village is null ? null : new VillageViewModel
//             {
//                 Id = address.Village.Id,
//                 Name = address.Village.Name
//             },
//             Street = address.Street is null ? null : new StreetViewModel
//             {
//                 Id = address.Street.Id,
//                 Name = address.Street.Name
//             },
//             Directions = address.Directions ?? string.Empty
//         };


//         public static IslandViewModel ToViewModel(this Island i) => new IslandViewModel
//         {
//             Id = i.Id,
//             Name = i.Name,
//             Parishes = i.Parishes != null ? i.Parishes.Select(p => new sys.models.ParishViewModel
//             {
//                 Id = p.Id,
//                 Name = p.Name,
//                 // TODO: consider adding villages and streets
//             }).ToList() : new List<ParishViewModel>(),
//         };


//         public static ParishViewModel ToViewModel(this Parish p) => new ParishViewModel
//         {
//             Id = p.Id,
//             Name = p.Name,
//             Island = p.Island != null ? new IslandViewModel
//             {
//                 Id = p.Island.Id,
//                 Name = p.Island.Name
//             } : null,
//             Villages = p.Villages?.Count > 0 ? p.Villages.Select(v => new VillageViewModel
//             {
//                 Id = v.Id,
//                 Name = v.Name,
//             }).ToList() : new List<VillageViewModel>(),
//         };


//         public static VillageViewModel ToViewModel(this Village v)
//         {
//             var _streets = v.Streets?.Count > 0 ? v.Streets.Select(s => new StreetViewModel
//             {
//                 Id = s.Id,
//                 Name = s.Name,
//             }).ToList() : new List<StreetViewModel>();

//             var _parish = v.Parish != null ? new ParishViewModel
//             {
//                 Id = v.Parish.Id,
//                 Name = v.Parish.Name,
//             } : null;

//             var _island = v.Island != null ? new IslandViewModel
//             {
//                 Id = v.Island.Id,
//                 Name = v.Island.Name,
//             } : (v.Parish?.Island != null ? new IslandViewModel
//             {
//                 Id = v.Parish.Island.Id,
//                 Name = v.Parish.Island.Name,
//             } : null);

//             if (_parish != null)
//             {
//                 _parish.Island = _island;
//             }

//             return new VillageViewModel
//             {
//                 Id = v.Id,
//                 Name = v.Name,
//                 Streets = _streets,
//                 Parish = _parish,
//                 Island = _island,
//             };
//         }


//         public static StreetViewModel ToViewModel(this Street st)
//         {

//             var _village = st.Village != null ? new VillageViewModel
//             {
//                 Id = st.Village.Id,
//                 Name = st.Village.Name,
//             } : null;

//             var _parish = st.Parish != null ? new ParishViewModel
//             {
//                 Id = st.Parish.Id,
//                 Name = st.Parish.Name,
//             } : (
//                 st.Village?.Parish != null ? new ParishViewModel
//                 {
//                     Id = st.Village.Parish.Id,
//                     Name = st.Village.Parish.Name,
//                 } : null
//             );

//             if (_village != null) { _village.Parish = _parish; }

//             var _island = st.Island != null ? new IslandViewModel
//             {
//                 Id = st.Island.Id,
//                 Name = st.Island.Name
//             } : (
//                 st.Parish?.Island != null ? new IslandViewModel
//                 {
//                     Id = st.Parish.Island.Id,
//                     Name = st.Parish.Island.Name
//                 } : (
//                     st.Village?.Island != null ? new IslandViewModel
//                     {
//                         Id = st.Village.Island.Id,
//                         Name = st.Village.Island.Name
//                     } : (
//                         st.Village?.Parish?.Island != null ? new IslandViewModel
//                         {
//                             Id = st.Village.Parish.Island.Id,
//                             Name = st.Village.Parish.Island.Name
//                         } : null
//                     )
//                 )
//             );


//             if (_parish != null) { _parish.Island = _island; }
//             if (_village != null) { _village.Island = _island; }


//             return new StreetViewModel
//             {
//                 Id = st.Id,
//                 Name = st.Name,
//                 Village = _village,
//                 Parish = _parish,
//                 Island = _island,
//             };
//         }
//     }
// }
