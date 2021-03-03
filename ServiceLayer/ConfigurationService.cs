using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Models.ViewModels;
using EIRLSSAssignment1.Models.enums;
using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.Common.Objects;
using Microsoft.AspNet.Identity;
using EIRLSSAssignment1.Common;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class ConfigurationService
    {
        private readonly ConfigurationRepository _configurationRepository;

        public ConfigurationService()
        {
            _configurationRepository = new ConfigurationRepository(new ApplicationDbContext());
        }

        public Configuration GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected an int");
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                throw new ConfigurationNotFoundException("Configuration not found.");
            }
            return configuration;
        }

        public ServiceResponse CreateAction(Configuration configuration)
        {

            configuration.IsConfigurationActive = true;

            DisableActiveConfiguration();

            _configurationRepository.Insert(configuration);
            _configurationRepository.Save();

            ServiceResponse response = new ServiceResponse {
                Result = true,
                ServiceObject = configuration
            };

            return response;
        }

        public Configuration EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected parameter");
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                throw new ConfigurationNotFoundException("Configuration not found.");
            }
            return configuration;
        }

        public ServiceResponse EditAction(Configuration configuration)
        {

            if (configuration.IsConfigurationActive == true)
            {
                DisableActiveConfiguration();
            }

            Configuration configToUpdate = _configurationRepository.GetConfigurationById(configuration.Id);

            configToUpdate.IsConfigurationActive = configuration.IsConfigurationActive;
            configToUpdate.Name = configuration.Name;
            configToUpdate.IsOpenForRentals = configuration.IsOpenForRentals;
            configToUpdate.OpeningTime = configuration.OpeningTime;
            configToUpdate.ClosingTime = configuration.ClosingTime;
            configToUpdate.MinRentalHours = configuration.MinRentalHours;
            configToUpdate.MaxRentalHours = configuration.MaxRentalHours;
            configToUpdate.LateReturnEligibility = configuration.LateReturnEligibility;
            configToUpdate.DvlaReference = configuration.DvlaReference;
            configToUpdate.DvlaImportUrl = configuration.DvlaImportUrl;
            configToUpdate.AbiImportUrl = configuration.AbiImportUrl;
            configToUpdate.PriceCheckUrl = configuration.PriceCheckUrl;
            configToUpdate.SmtpShouldSendEmail = configuration.SmtpShouldSendEmail;
            configToUpdate.SmtpUrl = configuration.SmtpUrl;
            configToUpdate.SmtpPort = configuration.SmtpPort;
            configToUpdate.SmtpSenderEmail = configuration.SmtpSenderEmail;
            configToUpdate.SmtpSenderPassword = configuration.SmtpSenderPassword;
            configToUpdate.SmtpShouldUseSsl = configuration.SmtpShouldUseSsl;
            configToUpdate.SmtpRecipientEmail = configuration.SmtpRecipientEmail;

            _configurationRepository.Update(configToUpdate);
            _configurationRepository.Save();

            ServiceResponse response = new ServiceResponse
            {
                Result = true,
                ServiceObject = configToUpdate
            };

            return response;

        }

        public Configuration DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected an int");
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                throw new ConfigurationNotFoundException("Configuration not found");
            }
            return configuration;
        }

        public ServiceResponse DeleteAction(int id)
        {
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            _configurationRepository.Delete(configuration);
            _configurationRepository.Save();
            return new ServiceResponse { Result = true };
        }

        public void Dispose()
        {
            _configurationRepository.Dispose();
        }

        public void DisableActiveConfiguration()
        {
            Configuration activeConfiguration = _configurationRepository.GetConfigurations().SingleOrDefault(c => c.IsConfigurationActive == true);

            if (activeConfiguration != null)
            {
                activeConfiguration.IsConfigurationActive = false;
                _configurationRepository.Update(activeConfiguration);
                _configurationRepository.Save();
            }
        }

    }
}