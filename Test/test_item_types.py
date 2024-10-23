import unittest
from httpx import Client

class Item_Types_Test(unittest.TestCase): #5
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_item_types(self):
        response = self.client.get('item_types')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_item_type(self):
        item_type_id = 1
        response = self.client.get(f'item_types/{item_type_id}')
        item_type = response.json()
        self.assertEqual(item_type['id'], item_type_id)
    
    def test_get_nonexistent_item_type(self):
        item_type_id = 1234567
        response = self.client.get(f'item_types/{item_type_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_item_type_by_id(self):
        response = self.client.get('item_types/500000000')
        item_type = response.json()
        self.assertIsNone(item_type)
    
    def test_get_item_type_invalid_path(self):
        response = self.client.get('/item_types/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    


if __name__ == '__main__':
    unittest.main()