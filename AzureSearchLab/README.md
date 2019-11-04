# Lab: Azure Search
In this file you'll create an Azure search service. Upload some files to a blob storage. Add enrichments to the data. Query the data and finally inspect what you've created.

## Upload images

Create a storage account if you don't have one you can use. 
Account kind: StorageV2 (general purpose)
Replication: Locally.redundant storage (LRS)

Create a container in your storage account called with a name e.g. randomfiles.

Upload the files from the [random images folder](/AzureSearchLab/randomimages)

## Create a search service

In the portal search for "Search services" and create a new Search service.

Go to the resource when it's deployed.

Click Import data and select your randomfiles data source.

Click Next to go to the Enrich content step.

Enter a name for the skillset e.g. randomimages

In Attach Cognitive Services select either a previously created resource or create a new one.

In Add Enrichments check the "Enable OCR and merge all text" checkbox.

Check all text and cogntive skills you want. In the Translate text you can choose e.g. Swedish.

Click Next: customize target index

You're now on the Customize target index step.

Set a name for the index e.g. randomfiles

Make people, organizations, locations, keyphrases filterable.
Make metadata_content_type sortable

Click Next: Create an indexer
Give the indexer a name e.g. randomfiles and submit.
Wait for the indexer to run. You can inspect the status by navigating to the Search Services resource overview and looking at the Indexer.

## Try some queries
Test searching for some queries. Like searching for something thats inside a document in the randomfiles folder or how an image looks.

To read more about search syntax checkout [Azure Search Simple Query Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/simple-query-syntax-in-azure-search) and [Lucene Query Syntax](https://docs.microsoft.com/en-us/azure/search/query-lucene-syntax)

## Inspect what you've created
Open postman and create a new get request.
Add a `api-key` header with the search service key as your value.
Make calls to
- `https://<searchname>.search.windows.net/indexers?api-version=2019-05-06-Preview`
- `https://<searchname>.search.windows.net/skillsets?api-version=2019-05-06-Preview`
- `https://<searchname>.search.windows.net/indexes?api-version=2019-05-06-Preview`

Inspect the results you get back.

