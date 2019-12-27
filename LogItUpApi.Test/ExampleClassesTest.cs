using LogItUpApi.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogItUpApi.Test
{
    [TestClass]
    public class ExampleClassesTest
    {
        //Esto es un MSTEST Project

        [TestMethod]
        public void TestMathClass()
        {
            //Preparación

            Exception ex = null;

            Math mathClass = new Math();

            //Prueba

            try
            {
                mathClass.DivideNumbers(1, 0);
            }
            catch (Exception e)
            {
                ex = e;
            }

            //Verificación

            Assert.IsTrue(ex is DivideByZeroException);

            Assert.AreEqual("El divisor debe ser distinto de 0", ex.Message);

        }

        [TestMethod]
        public void TestErrorCalculoPropinaClass()
        {
            //Preparación

            Exception ex = null;

            var mock = new Mock<IMath>();

            mock.Setup( x=> x.DivideNumbers(1,0))
                .Throws( new DivideByZeroException("El divisor debe ser distinto de 0"));
            
            CalculoPropina calculoPropina = new CalculoPropina(mock.Object);


            //Prueba

            try
            {
                calculoPropina.CalcularPropina(1, 0);
            }
            catch (Exception e)
            {
                ex = e;
            }

            //Verificación

            Assert.IsTrue(ex is DivideByZeroException);

            Assert.AreEqual("El divisor debe ser distinto de 0", ex.Message);

        }

        [TestMethod]
        public void TestSinErrorCalculoPropinaClass()
        {
            //Preparación

            var mock = new Mock<IMath>();

            mock.Setup(x => x.DivideNumbers(10, 2)).Returns(5);

            CalculoPropina calculoPropina = new CalculoPropina(mock.Object);

            //Prueba

            decimal result = calculoPropina.CalcularPropina(10, 2);

            //Verificación

            Assert.AreEqual(5, result);

        }

        [TestMethod]
        public void EnviarEmail()
        {
            var emailInfo = new EmailInfo()
            {
                SenderEmailAddress = "accounts@logitup.com",
                SenderName = "LogItUp",
                ReceiverEmailAddress = "manuelcolletti@hotmail.com",
                ReceiverName = "Manuel Colletti",
                Subject = "Confirmar cuenta",
                HtmlContent = "<h1> Esto es una prueba </h1>"
            };

            emailInfo.AddAttachment("descarga.jpg", File.ReadAllBytes("C:\\descarga.jpg"));

            new SendGridEmailSender().SendEmail(emailInfo).Wait();
        }

        [TestMethod]
        public void CrearPDF()
        {
            var strem = new HtmlToPdfGenerator().GeneratePdf("www.google.com", new HtmlToPdfConfig());

            byte[] bytes = strem.ToArray();

            File.WriteAllBytes(@"C:\Users\manuel.colleti\Desktop\test.pdf", bytes);
        }
    }

}
