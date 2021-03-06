﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class EFDBContext:DbContext
    {
        public DbSet<utblMstCountries> utblMstCountries { get; set; }
        public DbSet<utblMstState> utblMstStates { get; set; }
        public DbSet<utblMstDestination> utblMstDestinations { get; set; }
        public DbSet <utblLCMstCitie> utblLCMstCities { get; set; }
        public DbSet<utblLCMstLocalitie> utblLCMstLocalities { get; set; }
        public DbSet<utblLCMstAmenitie> utblLCMstAmenities { get; set; }
        public DbSet<utblLCMstStarRating> utblLCMstStarRatings { get; set; }
        public DbSet<utblLCMstHomeType> utblLCMstHomeTypes { get; set; }
        public DbSet<utblLCRoom> utblLCRooms { get; set; }
        public DbSet<utblLCHotel> utblLCHotels { get; set; }
        public DbSet<utblLCFeaturedHotel> utblLCFeaturedHotels { get; set; }
        public DbSet<utblLCHotelImage> utblLCHotelImages { get; set; }
        
        public DbSet<utblMstActivitie> utblMstActivities { get; set; }

        //package type
        public DbSet<utblMstPackageType> utblMstPackageTypes { get; set; }
        public DbSet<utblMstItinerarie> utblMstItineraries { get; set; }
        public DbSet<utblMstInclusion> utblMstInclusions{ get; set; }
        public DbSet<utblMstExclusion> utblMstExclusions{ get; set; }
        public DbSet<utblMstTerm> utblMstTerms { get; set; }
        public DbSet<utblMstBanner> utblMstBanners { get; set; }
        public DbSet<utblAgent> utblAgents { get; set; }
        public DbSet<utblPackageOffer> utblPackageOffers { get; set; }

        public DbSet<utblMstHotelType> utblMstHotelTypes { get; set; }
        public DbSet<utblMstHotel> utblMstHotels { get; set; }
        public DbSet<utblMstCabType> utblMstCabTypes { get; set; }
        public DbSet<utblMstCab> utblMstCabs { get; set; }
        public DbSet<utblMstCabDriver> utblMstCabDrivers { get; set; }
        public DbSet<utblMstCabHead> utblMstCabHeads { get; set; }

        public DbSet<utblMstTourCancellation> utblMstTourCancellations { get; set; } 
        public DbSet<utblTourPackage> utblTourPackages { get; set; }
        public DbSet<utblTourPackageItinerary> utblTourPackageItineraries { get; set; }
        public DbSet<utblTourPackageImage> utblTourPackageImages { get; set; }

        public DbSet<utblClientEnquirie> utblClientEnquiries { get; set; }
        public DbSet<utblClientEnquiryItinerarie> utblClientEnquiryItineraries { get; set; }
        public DbSet<utblClientEnquiryActivitie> utblClientEnquiryActivites { get; set; }

        public DbSet<utblTourGuide> utblTourGuides { get; set; }

        public DbSet<utblLCFeatureOffer> utblLCFeatureOffers { get; set; }
        public DbSet<utblLCMstHotelPremise> utblLCMstHotelPremises { get; set; }
        public DbSet<utblLCHelpPage> utblLCHelpPages { get; set; }
        public DbSet<utblLCNearByPoint> utblLCNearByPoints { get; set; }
        public DbSet<utblLCNotification> utblLCNotifications { get; set; }

        public DbSet<utblLCHotelLatLong> utblLCHotelLatLongs { get; set; }

        public DbSet<utblAboutU> utblAboutUs { get; set; }
        public DbSet<utblPolicie> utblPolicies { get; set; }
        public DbSet<utblPolicyPoint> utblPolicyPoints { get; set; }

        public DbSet<utblTrnUserOTP> utblTrnUserOTPs { get; set; }
    }
}
