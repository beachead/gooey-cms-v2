﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Persistence;
using System.Data;
using System.Data.SqlClient;
using Gooeycms.Data.Model.Subscription;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using System.Web.Security;
using Gooeycms.Business.Membership;
using Encore.PayPal.Nvp;
using Gooeycms.Business.Paypal;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Subscription
{
    public static class Registrations
    {
        public static String Encrypt(String text)
        {
            TextEncryption crypto = new TextEncryption();
            return crypto.Encrypt(text);
        }

        /// <summary>
        /// Checks the guid and verifies that it's currently valid.
        /// </summary>
        /// <param name="guid">The guid to check</param>
        /// <returns></returns>
        public static Boolean IsGuidValid(String guid)
        {
            Registration registration = FindExisting(guid, false);

            Boolean result = false;
            if ((registration != null) && (!registration.IsComplete))
                result = true;

            return result;
        }

        /// <summary>
        /// Finds an existing registration based upon the specified guid.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="createIfNecessary">Set to true to create a new registration if one doesn't exist.</param>
        /// <returns></returns>
        public static Registration FindExisting(String guid, Boolean createIfNecessary)
        {
            RegistrationDao dao = new RegistrationDao();
            Registration registration = dao.FindByGuid(guid);
            if ((createIfNecessary) && (registration == null))
            {
                registration = new Registration();
                registration.Guid = guid;
                registration.Created = UtcDateTime.Now;
            }

            return registration;
        }

        /// <summary>
        /// Saves or updates the registration
        /// </summary>
        /// <param name="registration"></param>
        public static void Save(Registration registration)
        {
            RegistrationDao dao = new RegistrationDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save(registration);
                tx.Commit();
            }
        }

        /// <summary>
        /// Converts the registration into a paid account
        /// accessible to the cms system and control panel.
        /// </summary>
        /// <param name="registration"></param>
        public static void ConvertToAccount(Registration registration)
        {
            MembershipUserWrapper wrapper = MembershipUtil.CreateFromRegistration(registration);
            if (wrapper == null)
                throw new ArgumentException("The subscription could not be created because the registration was not valid.");

            SubscriptionManager.CreateFromRegistration(registration);
            registration.IsComplete = true;
            Save(registration);
        }

        private static void Delete(Registration registration)
        {
            RegistrationDao dao = new RegistrationDao();
            using (Transaction tx = new Transaction())
            {
                dao.DeleteObject(registration);
                tx.Commit();
            }
        }

        public static Registration Load(string guid)
        {
            RegistrationDao dao = new RegistrationDao();
            return dao.FindByGuid(guid);
        }
    }
}
