using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiaConsulting.EO;
using SiaConsulting.EO.Abstractions;
using System.Collections.Generic;
using Spartademo;
using Spartademo.ContextLoaders;
using Spartademo.DTOs.Commands;
using Spartademo.DTOs.Events;
using Spartademo.Processors;

namespace spartademotest
{
    [TestClass]
    public class MieDispatcher
    {
        [TestMethod]
        public void RegisterBankprofil_From_Empty_Stream()
        {
            var mockStore = new List<IEvent>().AsReadOnly();
            var registerBankProfilCommand = new RegisterBankProfilCommand { ProfilId = "1", ServiceEndpoint = "Bank"};
            ContextLoaderBase commandContextLoader = new RegisterBankProfilCommandContextLoader();
            var context = commandContextLoader.Render(registerBankProfilCommand, mockStore);
            ICommandProcessor commandProcessor = new RegisterBankProfilCommandProcessor();
            var @events = commandProcessor.Process(registerBankProfilCommand, context);
            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(BankProfilRegistered));
            Assert.AreEqual(registerBankProfilCommand.ProfilId, ((BankProfilRegistered)@events[0]).ProfilId);
            Assert.AreEqual(registerBankProfilCommand.ServiceEndpoint, ((BankProfilRegistered)@events[0]).ServiceEndpoint);
        }

        [TestMethod]
        public void RegisterFitnessprofil_From_Empty_Stream()
        {
            var mockStore = new List<IEvent>().AsReadOnly();
            var registerFitnessProfilCommand = new RegisterFitnessProfilCommand { ProfilId = "2", ServiceEndpoint = "Samsung Gear 3" };
            ContextLoaderBase commandContextLoader = new RegisterFitnessProfilCommandContextLoader();
            var context = commandContextLoader.Render(registerFitnessProfilCommand, mockStore);
            ICommandProcessor commandProcessor = new RegisterFitnessProfilCommandProcessor();
            var @events = commandProcessor.Process(registerFitnessProfilCommand, context);
            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(FitnessProfilRegistered));
            Assert.AreEqual(registerFitnessProfilCommand.ServiceEndpoint, ((FitnessProfilRegistered)@events[0]).ServiceEndpoint);
        }

        [TestMethod]
        public void Zahlung_wurde_ausgefuehrt()
        {
            var mockStore = GetBankRegistrationTestData().AsReadOnly();
            var assignPaymentCommand = new AssignPaymentCommand { ProfilId = "1", ZahlungsId = "2", Summe = (decimal)5.80 };
            ContextLoaderBase commandContextLoader = new AssignPaymentCommandContextLoader();
            var context = commandContextLoader.Render(assignPaymentCommand, mockStore);
            ICommandProcessor commandProcessor = new AssignPaymentCommandProcessor();
            var @events = commandProcessor.Process(assignPaymentCommand, context);

            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(PaymentExecuted));
            Assert.AreEqual(assignPaymentCommand.ProfilId, ((PaymentExecuted)@events[0]).ProfilId);
            Assert.AreEqual(assignPaymentCommand.ZahlungsId, ((PaymentExecuted)@events[0]).ZahlungsId);
        }

        [TestMethod]
        public void Zahlung_wird_verweigert_aus_unbekannten_Gruenden()
        {
            var mockStore = GetBankRegistrationTestData().AsReadOnly();
            var assignPaymentCommand = new AssignPaymentCommand { ProfilId = "1", ZahlungsId = "-99", Summe = (decimal)5.80 };
            ContextLoaderBase commandContextLoader = new AssignPaymentCommandContextLoader();
            var context = commandContextLoader.Render(assignPaymentCommand, mockStore);
            ICommandProcessor commandProcessor = new AssignPaymentCommandProcessor();
            var @events = commandProcessor.Process(assignPaymentCommand, context);

            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(PaymentNotExecuted));
            Assert.AreEqual(assignPaymentCommand.ProfilId, ((PaymentNotExecuted)@events[0]).ProfilId);
            Assert.AreEqual(assignPaymentCommand.ZahlungsId, ((PaymentNotExecuted)@events[0]).ZahlungsId);
        }

        [TestMethod]
        public void Zahlung_nicht_moeglich_weil_Profil_nicht_vorhanden()
        {
            var mockStore = GetBankRegistrationTestData().AsReadOnly();
            var assignPaymentCommand = new AssignPaymentCommand { ProfilId = "-5", ZahlungsId = "1", Summe = (decimal)5.80 };
            ContextLoaderBase commandContextLoader = new AssignPaymentCommandContextLoader();
            var context = commandContextLoader.Render(assignPaymentCommand, mockStore);
            ICommandProcessor commandProcessor = new AssignPaymentCommandProcessor();
            var @events = commandProcessor.Process(assignPaymentCommand, context);

            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(BankProfileNotFound));
            Assert.AreEqual(assignPaymentCommand.ProfilId, ((BankProfileNotFound)@events[0]).ProfilId);
            Assert.AreEqual(assignPaymentCommand.ZahlungsId, ((BankProfileNotFound)@events[0]).ZahlungsId);
        }

        private static List<IEvent> GetBankRegistrationTestData()
        {
            return new List<IEvent>
            {
                new BankProfilRegistered {ProfilId = "1", ServiceEndpoint = "Bank"},
                new BankProfilRegistered {ProfilId = "-99", ServiceEndpoint = "Schufa-Verweigerungsbank"}
            };
        }

        private static List<IEvent> GetFitnessRegistrationTestData()
        {
            return new List<IEvent>
            {
                new FitnessProfilRegistered {ProfilId = "1", ServiceEndpoint = "Samsung Gear 3"},
                new FitnessProfilRegistered {ProfilId = "-99", ServiceEndpoint = "Funktionierendes iPhone"}
            };
        }

        [TestMethod]
        public void Naehrwerte_wurden_hinzugefügt()
        {
            var mockStore = GetFitnessRegistrationTestData().AsReadOnly();
            var assignNutritionalValuesCommand = new AssignNutritionalValuesCommand { ProfilId = "1", EnergyInKCal = 500 };
            ContextLoaderBase commandContextLoader = new AssignNutritionalValuesCommandContextLoader();
            var context = commandContextLoader.Render(assignNutritionalValuesCommand, mockStore);
            ICommandProcessor commandProcessor = new AssignNutritionalValuesCommandProcessor();
            var @events = commandProcessor.Process(assignNutritionalValuesCommand, context);

            Assert.IsNotNull(@events);
            Assert.AreEqual(1, @events.Count);
            Assert.IsInstanceOfType(@events[0], typeof(IEvent));
            Assert.IsInstanceOfType(@events[0], typeof(NutritionalValuesAdded));
            Assert.AreEqual(assignNutritionalValuesCommand.ProfilId, ((NutritionalValuesAdded)@events[0]).ProfilId);
            Assert.AreEqual(assignNutritionalValuesCommand.EnergyInKCal, ((NutritionalValuesAdded)@events[0]).EnergyInKCal);
        }
    }
}
