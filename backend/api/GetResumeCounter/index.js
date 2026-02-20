const { CosmosClient } = require("@azure/cosmos");

module.exports = async function (context, req) {
    const connectionString = process.env.COSMOS_CONNECTION_STRING;

    const client = new CosmosClient(connectionString);

    const database = client.database("cloudresume");
    const container = database.container("counter");

    const id = "1"; // your counter document ID

    // Read the current counter
    const { resource: item } = await container.item(id, id).read();

    let count = item.count || 0;
    count++;

    // Update the counter
    item.count = count;
    await container.items.upsert(item);

    context.res = {
        status: 200,
        body: { count: count }
    };
};
