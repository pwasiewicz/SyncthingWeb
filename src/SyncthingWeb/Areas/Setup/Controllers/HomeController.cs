﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyncthingWeb.Areas.Setup.Models;
using SyncthingWeb.Commands.Implementation.Settings;
using SyncthingWeb.Helpers;
using SyncthingWeb.Models;
using SyncthingWeb.Syncthing;

namespace SyncthingWeb.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class HomeController : ExtendedController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View(new SetupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Index(SetupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!await SyncthingContext.TestAccess(model.SyncthingConfigFile))
            {
                this.ModelState.AddModelError("SyncthingConfigFile", "No access to config file");
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            IdentityResult result;

            // TODO this.Logger.Info("Creating super admin user \"{0}\"", model.Email);
            result = await this.userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                //TODO this.Logger.Error(
                //    "Error while registering user \"{0}\": {1}",
                //    model.Email,
                //    string.Join(", ", result.Errors));

            }

            if (result.Succeeded)
            {
                await
                    this.CommandFactory.Create<UpdateGeneralSettingsCommand>()
                        .Setup(
                            s =>
                            {
                                s.Initialzed = true;
                                s.SyncthingConfigPath = model.SyncthingConfigFile;
                                s.AdminId = user.Id;
                                s.EnableRegistration = model.EnableRegistration;
                            })
                        .ExecuteAsync();

                await signInManager.SignInAsync(user, isPersistent: true);
                this.Notifications.NotifySuccess("Website was setup properly!");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            foreach (var err in result.Errors) this.Notifications.NotifyError(err.Description);
            return this.View(model);
        }



        public async Task<ActionResult> Configuration()
        {
            var vm = await this.CommandFactory.Create<GetCurrentGeneralSettingsCommand>().GetAsync();
            return this.View(vm);
        }

        [ValidateAntiForgeryToken, HttpPost, ActionName("Configuration")]
        public async Task<ActionResult> ConfigurationPOST(GeneralSettings model)
        {
            await this.CommandFactory.Create<UpdateGeneralSettingsCommand>().Setup(gs =>
            {
                gs.SyncthingConfigPath = model.SyncthingConfigPath;
                gs.EnableRegistration = model.EnableRegistration;
            }).ExecuteAsync();

            this.Notifications.NotifySuccess("Settings saved successfully.");
            return this.RedirectToAction("Configuration");
        }
    }
}