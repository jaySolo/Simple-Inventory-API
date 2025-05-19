// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using jsolo.simpleinventory.sys.models;
// using jsolo.simpleinventory.core.entities;
// using jsolo.simpleinventory.core.objects;

// namespace jsolo.simpleinventory.sys.extensions
// {
//     public static class ModelsExtentions
//     {
//         public static Customer ToEntity(
//             this CustomerViewModel model,
//             Address address,
//             Feeder feeder,
//             Inspector inspector,
//             DateTime creationDate,
//             string creatorIdStr,
// #nullable enable
//             DateTime? lastUpateDate = null,
//             string? lastUpatorIdStr = null
// #nullable disable
//         )
//         {
//             var equipment = model.Equipment.Select(e => e.ToEntity())?.ToList() ?? new List<Equipment>();

//             return new Customer(
//                 model.Id,
//                 model.AccountNo,
//                 model.CompanyName,
//                 new Name(model?.Title ?? "", model?.LastName ?? "", model?.FirstName ?? ""),
//                 equipment,
//                 address,
//                 feeder,
//                 inspector,
//                 model.Comments,
//                 creationDate,
//                 creatorIdStr,
//                 model.EquipmentApprovedOn,
//                 lastUpateDate,
//                 lastUpatorIdStr
//             );
//         }

//         public static Equipment ToEntity(this EquipmentViewModel model) => new Equipment(
//             model.Type,
//             model.Capacity,
//             model.Amount
//         );


//         public static Inspector ToEntity(
//             this InspectorViewModel model,

//             DateTime creationDate,
//             string creatorIdStr,
//             #nullable enable
//             DateTime? lastUpateDate = null,
//             string? lastUpatorIdStr = null
//             #nullable disable
//         ) => new Inspector (
//             model.Id,
//             model.EmployeeNo,
//             new Name(model.Title, model.LastName, model.FirstName),
//             creationDate,
//             creatorIdStr,
//             lastUpateDate,
//             lastUpatorIdStr
//         );


//         public static Feeder ToEntity(
//             this FeederViewModel model,
//             DateTime creationDate,
//             string creatorIdStr,
//             #nullable enable
//             DateTime? lastUpateDate = null,
//             string? lastUpatorIdStr = null
//             #nullable disable
//         ) => new Feeder(
//             model.Id,
//             model.Code,
//             model.Name,
//             creationDate,
//             creatorIdStr,
//             lastUpateDate,
//             lastUpatorIdStr
//         );
//     }
// }