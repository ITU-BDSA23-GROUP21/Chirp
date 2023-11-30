using Chirp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;





namespace Chirp.Razor.Pages;

public class InformationModel : TimelineModel {
    public InformationModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) {
    }


    protected override Task<List<CheepDto>> GetCheeps() {
        return _cheepService.GetCheeps(Pageno);
    }

}