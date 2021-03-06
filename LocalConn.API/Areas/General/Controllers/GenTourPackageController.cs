﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LocalConn.API.Models;
using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;

namespace LocalConn.API.Areas.General.Controllers
{

    [RoutePrefix("api/general/tourpackage")]
    public class GenTourPackageController : ApiController
    {
        dalTourPackage objDAL = new dalTourPackage();
        dalConfigurations objDalBanner = new dalConfigurations();
        
        [HttpGet]
        [Route("GenTourPackageList")]
        public async Task<GenTourPackageVM> GenTourPackageList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDAL.GetGenTourPackageListAsync(PageNo, PageSize, SearchTerm);
        }
        
        [HttpGet]
        [Route("GenTourPackageDtl")]
        public async Task<GenTourPackageDtlView> GetGenTourPackageDtl(string linktext)
        {
            return await objDAL.GetGenTourPackageDtlView(linktext);
        }
        [HttpGet]
        [Route("ItinerariesView")]
        public async Task<IEnumerable<PackageItineraryView>> ItinerariesView(long id)
        {
            return await objDAL.GetPackageItinerariesAsync(id);

        }

        [HttpGet]
        [Route("PackageInclusions")]
        public async Task<IEnumerable<PackageInclusions>> PackageInclusions(long id)
        {
            return await objDAL.GetPackageInclusionsAsync(id);
        }
        [HttpGet]
        [Route("PackageExclusions")]
        public async Task<IEnumerable<PackageExclusions>> PackageExclusions(long id)
        {
            return await objDAL.GetPackageExclusionsAsync(id);
        }
        [HttpGet]
        [Route("PackageActivities")]
        public async Task<IEnumerable<PackageActivities>> PackageActivites(long id)
        {
            return await objDAL.GetPackageActivitiesAsync(id);
        }
        [HttpGet]
        [Route("PackageTerms")]
        public async Task<IEnumerable<PackageTerms>> PackageTerms(long id)
        {
            return await objDAL.GetPackageTermsAsync(id);
        }
        [HttpGet]
        [Route("PackageCancellations")]
        public async Task<IEnumerable<PackageCancellations>> PackageCancellations(long id)
        {
            return await objDAL.GetPackageCancellationsAsync(id);
        }
        [HttpGet]
        [Route("GenTourPackageCoverImgView")]
        public async Task<GenTourPackageImage> GenTourPackageCoverImgView(long id)
        {
            return await objDAL.GenTourPackageCoverImgViewAsync(id);
        }
        [HttpGet]
        [Route("PackageGalleryImage")]
        public async Task<IEnumerable<GenTourPackageImage>> GenTourPackageGalImgView(long id)
        {
            return await objDAL.GenTourPackageGalImgViewAsync(id);
        }
        //for Homepage

        [HttpGet]
        [Route("homebannerlist")]
        public List<utblMstBanner> GetBannerList()
        {
            return objDalBanner.getBannerList();
        }
        [HttpGet]
        [Route("WhereNames")]
        public async Task<IEnumerable<string>> WhereNames()
        {
            return await objDAL.GetStateDestinationNamesAsync();
        }
        [HttpGet]
        [Route("TourTypes")]
        public async Task<IEnumerable<utblMstPackageType>> TourTypes()
        {
            return await objDalBanner.GetAllPackageTypesAsync();
        }
        [HttpPost]
        [Route("GenTourPackageSearch")]
        public async Task<GenTourPackageVM> GenTourPackageSearch(GenSearchModel model)
        {
            return await objDAL.SearchGenTourPackageListAsync(model);
        }
        [HttpGet]
        [Route("Destinations")]
        public async Task<IEnumerable<DestinationView>> Destinations()
        {
            return await objDAL.GetAllDestinationsAsync();
        }
        //for choosing Package Type to Display
        [HttpGet]
        [Route("GenTourPackageDispList")]
        public async Task<GenTourPackageVM> GenTourPackageDispList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDAL.GetGenTourPackageDispListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpGet]
        [Route("GenOfferPackagelist")]
        public async Task<IEnumerable<GenPackageOfferView>> GenOfferPackageList()
        {
            return await objDAL.GenGetOfferPackageListAsync();
        }
        [HttpGet]
        [Route("GenOfferPackageByID")]
        public async Task<GenTourPackageVM> GenTourPackageByID(long POID, int PageNo, int PageSize)
        {
            return await objDAL.GetGenOfferPackageListAsync(POID, PageNo, PageSize);
        }
       
    }
}
