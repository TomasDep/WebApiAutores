using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Core.Validators;

namespace WebAPIAutores.Tests.UnitTests;

[TestClass]
public class FirstLetterUppercaseTests
{
    [TestMethod]
    public void FirstLetterUppercase_Error()
    {
        var firstLetterUppercase = new FirstLetterUppercase();
        var valor = "don Quijote";
        var valContext = new ValidationContext(new { Nombre = valor });
        var result = firstLetterUppercase.GetValidationResult(valor, valContext);
        Assert.AreEqual("The first char needs to be uppercase", result.ErrorMessage);
    }

    [TestMethod]
    public void FirstLetterUppercase_ValueNull_NotError()
    {
        var firstLetterUppercase = new FirstLetterUppercase();
        string valor = null;
        var valContext = new ValidationContext(new { Nombre = valor });
        var result = firstLetterUppercase.GetValidationResult(valor, valContext);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FirstLetterUppercase_ValueFirstLetterUppercase_NotError()
    {
        var firstLetterUppercase = new FirstLetterUppercase();
        string valor = "Don Quijote";
        var valContext = new ValidationContext(new { Nombre = valor });
        var result = firstLetterUppercase.GetValidationResult(valor, valContext);
        Assert.IsNull(result);
    }
}