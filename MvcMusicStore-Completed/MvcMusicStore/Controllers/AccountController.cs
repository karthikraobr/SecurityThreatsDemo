using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc3ToolsUpdateWeb_Default.Models;
using MvcMusicStore.Models;

namespace Mvc3ToolsUpdateWeb_Default.Controllers
{
    public class AccountController : Controller
    {
        private static bool isMatched = false;
        MusicStoreEntities storeDB = new MusicStoreEntities();
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    MigrateShoppingCart(model.UserName);
                    Session["Logged"] = model.UserName;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    HttpCookie loggedUser = new HttpCookie("LoggedUsed", model.UserName);
                    //loggedUser.HttpOnly = true;
                    //loggedUser.Secure = true;
                    Response.Cookies.Add(loggedUser);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MigrateShoppingCart(model.UserName);
                    Session["Logged"] = model.UserName;
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        // [OptionalAuthorize(false)]
        // [Authorize]
       // [OptionalAuthorize(false)]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
       // [OptionalAuthorize(false)]
        //[Authorize]
        //[OptionalAuthorize(true)]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/AttackerChangeEmail
        public ActionResult AttackerChangeEmail()
        {
            return View();
        }

        //
        // POST: /Account/AttackerChangeEmail
        [HttpPost]
        public ActionResult AttackerChangeEmail(ArtistViewModel model)
        {
            var artist = new Artist();
            //  TryUpdateModel(artist);
            artist.Name = model.Name;
            artist.ArtistId = 1;
            //order.OrderDate = DateTime.Now;
            var original = storeDB.Artists.Find(18);
            //Save Order
            //original.ArtistId = 138;
            original.Name = model.Name;
            storeDB.Artists.Attach(original);
            var entry = storeDB.Entry(original);
            entry.Property(e => e.Name).IsModified = true;
            // other changed properties
            storeDB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ChangeUserEmail
        public ActionResult ChangeUserEmail()
        {
            return View();
        }
        //
        // POST: /Account/ChangeUserEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserEmail(ArtistViewModel model)
        {
            var artist = new Artist();
            //  TryUpdateModel(artist);
            artist.Name = model.Name;
            artist.ArtistId = 1;
            //order.OrderDate = DateTime.Now;
            var original = storeDB.Artists.Find(138);
            //Save Order
            //original.ArtistId = 138;
            original.Name = model.Name;
            storeDB.Artists.Attach(original);
            var entry = storeDB.Entry(original);
            entry.Property(e => e.Name).IsModified = true;
            // other changed properties
            storeDB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        //
        // GET: /Account/BruteForceAttacks
        public ActionResult BruteForceAttacks()
        {

            var timeStarted = DateTime.Now;
            //  Console.WriteLine("Start BruteForce - {0}", timeStarted.ToString());

            // The length of the array is stored permanently during runtime
            charactersToTestLength = charactersToTest.Length;

            // The length of the password is unknown, so we have to run trough the full search space
            var estimatedPasswordLength = 0;

            while (!isMatched)
            {
                /* The estimated length of the password will be increased and every possible key for this
                 * key length will be created and compared against the password */
                estimatedPasswordLength++;
                startBruteForce(estimatedPasswordLength);
            }
            ViewBag.result = result;
            ViewBag.TimeTaken = Convert.ToString(DateTime.Now.Subtract(timeStarted).TotalSeconds);
            //System.Diagnostics.Debug.WriteLine("Password matched. - {0}", DateTime.Now.ToString());
            //System.Diagnostics.Debug.WriteLine("Time passed: {0}s", DateTime.Now.Subtract(timeStarted).TotalSeconds);
            //System.Diagnostics.Debug.WriteLine("Resolved password: {0}", result);
            //System.Diagnostics.Debug.WriteLine("Computed keys: {0}", computedKeys);
            return View();
        }
        #region Password Generate methods

        /// <summary>
        /// Starts the recursive method which will create the keys via brute force
        /// </summary>
        /// <param name="keyLength">The length of the key</param>
        private static void startBruteForce(int keyLength)
        {
            var keyChars = createCharArray(keyLength, charactersToTest[0]);
            // The index of the last character will be stored for slight perfomance improvement
            var indexOfLastChar = keyLength - 1;
            createNewKey(0, keyChars, keyLength, indexOfLastChar);
        }

        /// <summary>
        /// Creates a new char array of a specific length filled with the defaultChar
        /// </summary>
        /// <param name="length">The length of the array</param>
        /// <param name="defaultChar">The char with whom the array will be filled</param>
        /// <returns></returns>
        private static char[] createCharArray(int length, char defaultChar)
        {
            return (from c in new char[length] select defaultChar).ToArray();
        }

        /// <summary>
        /// This is the main workhorse, it creates new keys and compares them to the password until the password
        /// is matched or all keys of the current key length have been checked
        /// </summary>
        /// <param name="currentCharPosition">The position of the char which is replaced by new characters currently</param>
        /// <param name="keyChars">The current key represented as char array</param>
        /// <param name="keyLength">The length of the key</param>
        /// <param name="indexOfLastChar">The index of the last character of the key</param>
        private static void createNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar)
        {
            var nextCharPosition = currentCharPosition + 1;
            // We are looping trough the full length of our charactersToTest array
            for (int i = 0; i < charactersToTestLength; i++)
            {
                /* The character at the currentCharPosition will be replaced by a
                 * new character from the charactersToTest array => a new key combination will be created */
                keyChars[currentCharPosition] = charactersToTest[i];

                // The method calls itself recursively until all positions of the key char array have been replaced
                if (currentCharPosition < indexOfLastChar)
                {
                    createNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar);
                }
                else
                {
                    // A new key has been created, remove this counter to improve performance
                    computedKeys++;

                    /* The char array will be converted to a string and compared to the password. If the password
                     * is matched the loop breaks and the password is stored as result. */
                    if ((new String(keyChars)) == password)
                    {
                        if (!isMatched)
                        {
                            isMatched = true;
                            result = new String(keyChars);
                        }
                        return;
                    }
                }
            }
        }

        #endregion
        #region Password Generate variables

        // the secret password which we will try to find via brute force
        //private static string password = "p123";
        private static string password = "5434";
        private static string result;



        /* The length of the charactersToTest Array is stored in a
         * additional variable to increase performance  */
        private static int charactersToTestLength = 0;
        private static long computedKeys = 0;

        /* An array containing the characters which will be used to create the brute force keys,
         * if less characters are used (e.g. only lower case chars) the faster the password is matched  */
        private static char[] charactersToTest =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
        'F','G','H','I','J','K','L','M','N','O','P','Q','R',
        'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
        '6','7','8','9','0','!','$','#','@','-'
    };
        #endregion

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
