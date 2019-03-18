## Project Info
- Built with [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.1) and [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- Hosted on Heroku as Docker Container: https://netcore2heroku-docker.herokuapp.com/swagger
- Code Repo: https://github.com/bflow/MovieProject
- Article about migrating this API from Azure to Heroku: https://bit.ly/2XYkYCA

## Project Goals
- Demonstrate ASP.NET Identity authentication
- Demonstrate Kestrel database against Entity Framework Core Code-First DAL
- Demonstrate .NET Core deployments to Heroku using Docker Linux container 

## Using the API
**_Summary:_** 
This API offers endpoints for user creation/authentication, movie database search, movie rental, plus movie search and rental activity by product and user. 

**_Methods:_**
```
/api/Login/login/{userName}/{pwd}
```
Authenticates created user against the ASPdotNET Core Identity framework. 
```
/api/Login/create/{userName}/{pwd}
```
Creates and authenticates a new user with password using the ASPdotNET Core Identity framework. Password creation is quite lax for test purposes but can be easily configured for more astringency in the Startup class.
```
/api/Login/logout 
```
Logs out (unauthenticates) a created user using the ASPdotNET Core Identity framework. 
```
/api/Movie/search/{searchText}
```
Free-form search of the [The Movie Database](https://www.themoviedb.org/?language=en-US) using the [TMDBLib NuGet package](https://www.nuget.org/packages/Tmdblib). Returns a collection of movies and metadata serialized into a JSON string. After every search, the search terms and results are collected per authenticated user. Search counts from both unauthenticated and authenticated searches are incremented for each movie in the search resultset.
```
/api/Movie/events/{locatorID}
```
Returns the event counts for the movie corresponding to the locatorID. Rentals and Searches are events. 
```
/api/Movie/user/events/{userName}  
```
Returns the search terms, search results, and event counts for the movie indicated by locatorID. Rentals and Searches are events. 
```
/api/Movie/rent/{locatorID} 
```
Registers a Rental event against the authenticated user and increments the product's Rental event counter.

## Usage
**Sample Demonstration Workflow:**
1. Browse to https://netcore2heroku-docker.herokuapp.com/swagger
1. Use the **Movie/search/{searchText}** route to search against the term 'fletch' (without quotes.) Note the id (9749) of the first movie in the resultset.
3. Use the **Movie/events/{locatorID}** route to search on locator 9749. Observe that the previous search has been counted for this movie. 
4. Use the **Login/create/{userName}/{pwd}** route to create a username and password. Password must be at least six characters and are limited to alphanumerics plus the characters -._@+. This user will be auto-authenticated.
5. Repeat Step 2, then use the **Login/user/events/{userName}** route to search on your username. You should see one only resultset from the search in the previous step.
7. Repeat Step 3. You should now see that movie 9749 has been searched on twice.
8. Use the **Movie/rent/{locatorID}** route to rent movie 9749, then repeat Step 3. Observe that movie 9749 has been searched on twice and rented once.
9. Repeat Step 6. Observe that the is a Rental event logged against your user.
10. Use **Login/logout** route to unauthenticate your user, then repeat Steps 2, 3, and 6. 
1. You should see that three searches have been conducted against movie 9749, but Step 6 shows only two searches of the same movie from your authenticated user. This shows that your user was indeed logged out. 
