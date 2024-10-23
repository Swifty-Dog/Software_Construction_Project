import unittest
from httpx import Client

class Item_Groups_Test(unittest.TestCase): 
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_item_groups(self):
        response = self.client.get('item_groups')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_item_group(self):
        item_group_id = 1
        response = self.client.get(f'item_groups/{item_group_id}')
        item_group = response.json()
        self.assertEqual(item_group['id'], item_group_id)
    
    def test_get_nonexistent_item_group(self):
        item_group_id = 1234567
        response = self.client.get(f'item_groups/{item_group_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_item_group_by_id(self):
        response = self.client.get('item_groups/500000000')
        item_group = response.json()
        self.assertIsNone(item_group)

    def test_get_item_group_invalid_path(self):
        response = self.client.get('/item_groups/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    
    # def test_post_item_group(self):
    #     self.base_url = 'http://localhost:3000/api/v1/'
    #     response = self.client.post(self.base_url + 'item_groups', json={
    #         "id": 300000,
    #         "name": "Electronics",
    #         "description": "TEARRR",
    #         "created_at": "1998-05-15 19:52:53",
    #         "updated_at": "2000-11-20 08:37:56"
    #     })
    #     self.assertEqual(response.status_code, 201)
    
    


if __name__ == '__main__':
    unittest.main()