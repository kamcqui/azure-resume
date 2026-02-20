window.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
})

// Since it's a Static Web App, /api is a relative route. 
// This works both locally (via SWA CLI) and in production.
const functionApiURL = '/api/GetResumeCounter'; 

const getVisitCount = () => {
    let count = 30; // Your default/fallback count
    fetch(functionApiURL).then(response => {
        return response.json()
    }).then(response => {
        console.log("Website called function API.");
        // NOTE: C# property 'Count' usually serializes to 'count' in JSON
        count = response.count; 
        document.getElementById("counter").innerText = count;
    }).catch(function(error) {
        console.log("Error calling the API:", error);
    });
    return count;
}