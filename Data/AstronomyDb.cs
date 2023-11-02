using Microsoft.JSInterop;
using SQLite;
using sum200_project_stargazer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sum200_project_stargazer.Data
{
    public class AstronomyDb
    {
        //Code reference https://blazorhelpwebsite.com/ViewBlogPost/61

        string _dbPath;

        public string StatusMessage { get; set; }
        private SQLiteAsyncConnection conn;

        public AstronomyDb(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task InitializeDatabase()
        {
            try
            {
                if (conn != null)
                    return;

                // Create database and tables if they don't exist
                conn = new SQLiteAsyncConnection(_dbPath);
                await conn.CreateTableAsync<ApodImage>();
                await conn.CreateTableAsync<CollectionList>();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to initialize database. ", ex.Message);
            }
        }

        public async Task<ApodImage> GetImageById(string id)
        {
            try
            {
                await InitializeDatabase();
                return await conn.FindAsync<ApodImage>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get collection image. ", ex.Message);
                return null;
            }
        }

        public async Task<List<ApodImage>> GetImages()
        {
            try
            {
                await InitializeDatabase();
                return await conn.Table<ApodImage>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get images. ", ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveImage(ApodImage apodImage)
        {
            try
            {
                await InitializeDatabase();
                //Check if an object with the same URL exists
                ApodImage existingImage = await conn.FindWithQueryAsync<ApodImage>("SELECT * FROM ApodImage WHERE Url = ?", apodImage.Url);

                if (existingImage == null)
                {
                    await conn.InsertAsync(apodImage);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to save image. ", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteImage(ApodImage apodImage)
        {
            try
            {
                await InitializeDatabase();
                await conn.DeleteAsync(apodImage);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete image. ", ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangeImageCollection(string imageId, string newCollectionId)
        {
            try
            {
                await InitializeDatabase();
                ApodImage apodImage = await conn.FindAsync<ApodImage>(imageId);
                apodImage.CollectionListId = newCollectionId;
                await conn.UpdateAsync(apodImage);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to change image collection. ", ex.Message);
                return false;
            }

        }

        public async Task<List<CollectionList>> GetCollectionLists()
        {
            try
            {
                await InitializeDatabase();
                return await conn.Table<CollectionList>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get collection lists. ", ex.Message);
                return null;
            }
        }

        public async Task<CollectionList> GetCollectionListById(string id)
        {
            try
            {
                await InitializeDatabase();
                return await conn.FindAsync<CollectionList>(id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get collection list. ", ex.Message);
                return null;
            }
        }

        public async Task<bool> CreateCollectionList(string title)
        {
            try
            {
                await InitializeDatabase();
                CollectionList collectionList = new CollectionList();
                collectionList.Id = Guid.NewGuid().ToString();
                collectionList.Title = title;
                await conn.InsertAsync(collectionList);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to create collection list. ", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCollectionList(CollectionList collectionList)
        {
            try
            {
                await InitializeDatabase();
                await UnassignImagesFromCollection(collectionList.Id);
                await conn.DeleteAsync(collectionList);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete collection list. ", ex.Message);
                return false;
            }
        }

        private async Task UnassignImagesFromCollection(string collectionId)
        {
            try
            {
                await InitializeDatabase();
                List<ApodImage> images = await conn.Table<ApodImage>().Where(x => x.CollectionListId == collectionId).ToListAsync();
                foreach (ApodImage image in images)
                {
                    image.CollectionListId = null;
                    await conn.UpdateAsync(image);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to unassign images from collection. ", ex.Message);
            }
        }

        public async Task<bool> EditCollectionList(string id, string newTitle)
        {
            try
            {
                await InitializeDatabase();
                CollectionList collectionList = await conn.FindAsync<CollectionList>(id);
                collectionList.Title = newTitle;
                await conn.UpdateAsync(collectionList);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to edit collection list. ", ex.Message);
                return false;
            }
        }
    }
}
