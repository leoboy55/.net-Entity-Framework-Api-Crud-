using ApiCase.Data;
using ApiCase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Web;

namespace ApiCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AddresController : ControllerBase
    {
        private readonly DataContext _context;

        public AddresController(DataContext context)
        {
            _context = context;
        }

        //get all the addresses
        [HttpGet]
        public async Task<ActionResult<List<Addres>>> GetAllAddresses()
        {
            var addresses = await _context.Addresses.ToListAsync();

            return Ok(addresses);
        }

        //get sorted addres list A-Z
        [HttpGet("Sort StreetName A-Z")]
        public async Task<ActionResult<List<Addres>>> GetAllAddressesSortedAZ()
        {
            var addresses = await _context.Addresses.ToListAsync();

            List<Addres> sortedAdresses = addresses;
        
            sortedAdresses = addresses.OrderBy(x => x.Street).ToList();
            
            return Ok(sortedAdresses);
        }

        //get sorted addres list Z-A
        [HttpGet("Sort StreetName Z-A")]
        public async Task<ActionResult<List<Addres>>> GetAllAddressesSortedZA()
        {
            var addresses = await _context.Addresses.ToListAsync();

            List<Addres> sortedAdresses = addresses;

            sortedAdresses = addresses.OrderByDescending(x => x.Street).ToList();

            return Ok(sortedAdresses);

        }

        //Sort Addres if there is match
        [HttpGet("Search Addresses")]
        public async Task<ActionResult<List<Addres>>> Search(string field)
        {
            var addres = await _context.Addresses.ToListAsync();

            var checkStreetCount = addres.Where(x => x.Street == field).Count();
            var checkHouseNumberCount = addres.Where(x => x.HouseNumber.ToString() == field).Count();
            var checkPostalCount = addres.Where(x => x.PostalNumber == field).Count();
            var checkCityCount = addres.Where(x => x.PostalNumber == field).Count();
            var checkCountryCount = addres.Where(x => x.Country == field).Count();

            if(checkStreetCount > 0)
            {
                var getMatchingStreets = addres.Where(x => x.Street == field).ToList();
                return Ok(getMatchingStreets);
            } 
            else if (checkHouseNumberCount > 0)
            {
                var getMatchingHouseNumbers = addres.Where(x => x.HouseNumber.ToString() == field).ToList();
                return Ok(getMatchingHouseNumbers);
            }
            else if (checkPostalCount > 0)
            {
                var getMatchingPostals = addres.Where(x => x.PostalNumber == field).ToList();
                return Ok(getMatchingPostals);
            } 
            else if (checkCityCount > 0)
            {
                var getMatchingCities = addres.Where(x => x.City == field).ToList();
                return Ok(getMatchingCities);
            }
            else if (checkCountryCount > 0)
            {
                var getMatchingCountries = addres.Where(x => x.Country == field).ToList();
                return Ok(getMatchingCountries);
            } 
            else
            {
                return BadRequest("Addres value not found.");
            }
        }


        //get single addres
        [HttpGet("id")]
        public async Task<ActionResult<Addres>> GetAddres(int id)
        {
            var addres = await _context.Addresses.FindAsync(id);

            if (addres == null)
            {
                return BadRequest("Addres not found.");
            }
            return Ok(addres); 
        }


        //post an addres
        [HttpPost]
        public async Task<ActionResult<Addres>> AddAdres(Addres addres)
        {
            _context.Addresses.Add(addres);

            await _context.SaveChangesAsync();

            return Ok(addres);
        }

        //Update an addres
        [HttpPut]
        public async Task<ActionResult<Addres>> UpdateAddres(Addres request)
        {
            var addres = await _context.Addresses.FindAsync(request.Id);

            if (addres == null)
            {
                return BadRequest("Addres not found.");
            }

            addres.Street = request.Street;
            addres.HouseNumber = request.HouseNumber;
            addres.PostalNumber = request.PostalNumber;
            addres.City = request.City;
            addres.Country = request.Country;

            await _context.SaveChangesAsync();

            return Ok(addres);
        }

        //delete an addres
        [HttpDelete("id")]
        public async Task<ActionResult<Addres>> DeleteAddres(int id)
        {
            var addres = await _context.Addresses.FindAsync(id);

            if (addres == null)
            {
                return BadRequest("Addres not found.");
            }

            _context.Addresses.Remove(addres);
            await _context.SaveChangesAsync();

            return Ok(addres);
        }

        //find the distance between 2 addresses
        [HttpGet("distance")]
        public async Task<ActionResult<Addres>> CallDistanceApi(int adressId, int adressId2)
        {
            //Get all addresses from the DB
            var addressesList = await _context.Addresses.ToListAsync();

            //Filter the addresses from the 1st paramater -> store the addres
            var adress1 = addressesList.Where(x => x.Id == adressId).FirstOrDefault();

            //Filter the addresses from the 2nd paramter -> store the addres
            var adress2 = addressesList.Where(x => x.Id == adressId2).FirstOrDefault();

            //Validate if 1st paramter and 2 paramater is a mtch in the DB
            if (adress1 == null && adress2 == null)
            {
                return BadRequest("Addresses not found.");
            }
            else if (adress1 != null && adress2 == null)
            {
                return BadRequest("Addres 1 found, Addres 2 not found.");
            }
            else if (adress1 == null && adress2 != null)
            {
                return BadRequest("Addres 1 not found, Addres 2 found.");
            }

            //Start an Api call client
            using (HttpClient client = new HttpClient())
            {
                //Base addres
                var baseUrl = "https://api.distancematrix.ai/maps/api/distancematrix/";

                //Start a query string and make it empty inside
                var queryParameters = new Dictionary<string, string>();

                var accesToken = "uAq8Du76yObrnpW9ZGLc1ngKmv118";
                //Add acces token.
                queryParameters.Add("key", accesToken);

                //Add from addres.
                queryParameters.Add("origins", $"{adress1.Street} {adress1.HouseNumber},{adress1.City} {adress1.PostalNumber},{adress1.Country}");

                //Add destination addres.
                queryParameters.Add("destinations", $"{adress2.Street} {adress2.HouseNumber},{adress2.City} {adress2.PostalNumber},{adress2.Country}");


                //send the url to the website
                var newUrl = new Uri(QueryHelpers.AddQueryString(baseUrl + "json?", queryParameters));
                using (HttpResponseMessage response = await client.GetAsync(newUrl))
                {
                    //catch the results from the api
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
            }
        }
    }
}
