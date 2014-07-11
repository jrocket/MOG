using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{

    public class HomeController : FlabbitController
    {
        private IProjectService serviceProject = null;
        private IInviteMeService serviceInvit = null;
        private IFileService serviceFile = null;
        public HomeController(IInviteMeService invitService, IProjectService projectService
            ,IUserService userService
            ,IFileService fileService
             , ILogService logService
             )
            : base(userService, logService)
        {
            this.serviceInvit = invitService;
            this.serviceFile = fileService;
            this.serviceProject = projectService;
        }
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //    return RedirectToAction("New", "Project");
            var recentProject = this.serviceProject.GetNew(1, 12, true)
                .ToList();
            var fileIds = recentProject.Select(p => p.PromotedId);
            List<ProjectFile> files = this.serviceFile.GetByIds(fileIds);
            List<UserProfileInfo> recentMembers = this.serviceUser.GetNew(1, 4, true).ToList();
            HomeVM model = new HomeVM();
            model.Projects = recentProject;
            model.Users = recentMembers;
            model.Files = files;
            
            return View(model);
        }

        [HttpPost]
        public JsonResult InviteMe(string email)
        {
            int result = this.serviceInvit.InviteMe(email, getIPAddress(Request));
            return new JsonResult() { Data = result != -1 };
        }

        private string getIPAddress(HttpRequestBase request)
        {
            try
            {
                var userHostAddress = request.UserHostAddress;

                // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                // Could use TryParse instead, but I wanted to catch all exceptions
                System.Net.IPAddress.Parse(userHostAddress);

                var xForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(xForwardedFor))
                    return userHostAddress;

                // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                // If we found any, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }
        }

        private bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = System.Net.IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        public ActionResult Offline()
        {
            return View();
        }
    }
}