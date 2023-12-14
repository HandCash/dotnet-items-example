## Example Handcash Items Client 

### Getting started
Add your configuration to app settings.  You can find these credentials at https://dashboard.handcash.io
```
  "HandcashConfig": {
    "BaseUrl": "https://cloud.handcash.io",
    "AppId": "<app-id>",
    "AppSecret": "<app-secret>",
    "AuthToken": "<auth-token>"
  }
```
A full description of the item metadata is here https://docs.handcash.io/docs/collection-metadata

### important notes
- using this new endpoint minting is free and we are limiting file sizes to 50kb and batch sizes of 100 items per batch
- you provide 2 images 
  - `item.mediaDetails.image.url` is a link to the image that is put on chain with a limit of 50kb
  - `item.mediaDetails.image.imageHighResUrl` is a link to an image that Handcash will cache to display in market and portal and this does not have a size limit 


### Functions Description

### `CreateCollectionItemOrder`

- **Purpose**: Creates an order to add items to a collection in the background.
- **Parameters**:
  - `collectionId`: The ID of the collection to add items to.
  - `itemsToCreate`: A list of item metadata for creation.
- **Returns**: An instance of `CreateItemsOrder` containing details about the creation order, including its ID.
- **Performance Note**: Approximately 15 seconds for 100 items at 50kb each, and around 3 seconds for 10 items at 50kb each.

### `GetOrderStatus`

- **Purpose**: Retrieves the current status of an item creation order.
- **Parameters**:
  - `orderId`: The ID of the order to check.
- **Returns**: An updated `CreateItemsOrder` object reflecting the current status of the order.
- **Usage**: Useful for tracking the progress or completion of an item creation order.

### `GetItemsByOrder`

- **Purpose**: Fetches the list of items associated with a specific creation order.
- **Parameters**:
  - `orderId`: The ID of the order whose items are to be retrieved.
- **Returns**: A list of `Item` objects that were part of the specified order.
- **Usage**: Allows retrieval of all items created as part of a specific order, once the order is completed.

### General Notes

- These functions are part of a client library designed to interact with the Handcash cloud API.
- Ensure proper authentication and error handling when using these functions.
