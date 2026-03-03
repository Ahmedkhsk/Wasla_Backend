#region ASP.NET Core & Framework
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Caching.Memory;
#endregion

#region Machine Learning
global using Microsoft.ML;
global using Microsoft.ML.Data;
#endregion

#region System
global using System;
global using System.IO;
global using System.Text;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Reflection;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Data;
global using System.Drawing;
global using System.Drawing.Imaging;
#endregion

#region Third-Party Libraries
global using AutoMapper;
global using MailKit.Security;
global using MimeKit;
global using MimeKit.Text;
global using QRCoder;
global using Hangfire;
global using FirebaseAdmin;
global using Google.Apis.Auth.OAuth2;
global using FirebaseAdmin.Messaging;

#endregion

#region Project - DTOs
global using Wasla_Backend.DTOs;
global using Wasla_Backend.DTOs.Authentication;
global using Wasla_Backend.DTOs.AdminDTOS;
global using Wasla_Backend.DTOs.DoctorDTO;
global using Wasla_Backend.DTOs.ResidentDTOS;
global using Wasla_Backend.DTOs.RoleDTOS;
global using Wasla_Backend.DTOs.MlDTOS;
global using Wasla_Backend.DTOs.ServiceDTOS;
global using Wasla_Backend.DTOs.ReviewDtos;
global using Wasla_Backend.DTOs.BookDTOS;
global using Wasla_Backend.DTOs.HubsDto;
global using Wasla_Backend.DTOs.FavouritsDTOS;
global using Wasla_Backend.DTOs.ChartDTOS;
global using Wasla_Backend.DTOs.GymDTOS;
global using Wasla_Backend.DTOs.UserEventDTOS;
global using Wasla_Backend.DTOs.NotificationDTOS;
global using Wasla_Backend.DTOs.SoicalDTOS;
global using Wasla_Backend.DTOs.DriverDTOS;
#endregion

#region Project - Models
global using Wasla_Backend.Models;
global using Wasla_Backend.Models.BaseModel;
global using Wasla_Backend.Models.GeneralModel;
global using Wasla_Backend.Models.GymModel;
global using Wasla_Backend.Models.Social;
global using Wasla_Backend.Models.Driver;


#endregion

#region Project - Repositories
global using Wasla_Backend.Repositories.Interfaces;
global using Wasla_Backend.Repositories.Interfaces.Gyms;
global using Wasla_Backend.Repositories.Implementation;
global using Wasla_Backend.Repositories.Interfaces.driver;
global using Wasla_Backend.Repositories.Interfaces.General;


#endregion

#region Project - Services
global using Wasla_Backend.Services.Interfaces;
global using Wasla_Backend.Services.Interfaces.General;
global using Wasla_Backend.Services.Interfaces.GymService;
global using Wasla_Backend.Services.Implementation;
global using Wasla_Backend.Services.Implementation.General;
global using Wasla_Backend.Services.Interfaces.Driver;


#endregion

#region Project - Factories
global using Wasla_Backend.Factories.Interfaces;
global using Wasla_Backend.Factories.Implementation;
#endregion

#region Project - Helpers
global using Wasla_Backend.Helpers;
global using Wasla_Backend.Helpers.Response;
global using Wasla_Backend.Helpers.Localization;
global using Wasla_Backend.Helpers.EmailSender;
global using Wasla_Backend.Helpers.File;
global using Wasla_Backend.Helpers.MlHelper;
global using Wasla_Backend.Helpers.Resolvers;
global using Wasla_Backend.Helpers.Time;
global using Wasla_Backend.Helpers.Cashing;
global using Wasla_Backend.Helpers.NotificationHelper;
global using Wasla_Backend.Helpers.Hangfire;


#endregion

#region Project - Program Helpers
global using Wasla_Backend.Helpers.ProgramHelper.Configurations;
global using Wasla_Backend.Helpers.ProgramHelper.DependencyInjection;
global using Wasla_Backend.Helpers.ProgramHelper.Pipeline;
#endregion

#region Project - Hubs
global using Wasla_Backend.Hubs.BookingHubs;
global using Wasla_Backend.Hubs.ServiceHubs;
global using Wasla_Backend.Hubs.ReviewHubs;
#endregion

#region Project - Data
global using Wasla_Backend.Data;
#endregion

#region Project - Middlewares
global using Wasla_Backend.Middlewares;
#endregion

#region Project - Enums
global using Wasla_Backend.Enums;
#endregion

#region Project - Exceptions
global using Wasla_Backend.Exceptions;
#endregion