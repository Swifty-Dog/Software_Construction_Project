import unittest
from httpx import Client

class Inventories_Test(unittest.TestCase): 
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_inventory_authentication(self):
        self.client_fail = Client(base_url='http://localhost:3000/api/v1/')
        response = self.client_fail.get('inventories')
        self.assertEqual(response.status_code, 401)

    def test_get_inventories(self):
        response = self.client.get('inventories')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_inventory(self):
        inventory_id = 1
        response = self.client.get(f'inventories/{inventory_id}')
        inventory = response.json()
        self.assertEqual(inventory['id'], inventory_id)
    
    def test_get_nonexistent_inventory(self):
        inventory_id = 1234567
        response = self.client.get(f'inventories/{inventory_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_inventory_by_id(self):
        response = self.client.get('inventories/500000000')
        inventory = response.json()
        self.assertIsNone(inventory)
    
    def test_get_inventory_with_invalid_path(self):
        response = self.client.get('/inventories/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    
    def test_post_inventory(self):
        inventory_data = {
        "id": 98765432,
        "item_id": "P000001",
        "description": "Face-to-face clear-thinking complexity",
        "item_reference": "TEST",
        "locations": [
            3211,
            24700,
            14123,
            19538,
            31071,
            24701,
            11606,
            11817
        ],
        "total_on_hand": 262,
        "total_expected": 0,
        "total_ordered": 80,
        "total_allocated": 41,
        "total_available": 141,
        "created_at": "2015-02-19 16:08:24",
        "updated_at": "2015-09-26 06:37:56"
    }
        post_response = self.client.post('inventories', json=inventory_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('inventories/98765432')
        inventory = get_response.json()
        self.assertEqual(inventory['item_reference'], "TEST")
        self.assertEqual(inventory['item_id'], "P000001")
    
    def test_post_inventory_success_with_length(self):
        response = self.client.get('inventories')
        old_inventory_length = len(response.json())
        inventory_data = {
        "id": 8888888,
        "item_id": "P000001",
        "description": "Face-to-face clear-thinking complexity",
        "item_reference": "TEST",
        "locations": [
            3211,
            24700,
            14123,
            19538,
            31071,
            24701,
            11606,
            11817
        ],
        "total_on_hand": 262,
        "total_expected": 0,
        "total_ordered": 80,
        "total_allocated": 41,
        "total_available": 141,
        "created_at": "2015-02-19 16:08:24",
        "updated_at": "2015-09-26 06:37:56"
    }
        post_response = self.client.post('inventories', json=inventory_data)
        self.assertEqual(post_response.status_code, 201)
        response = self.client.get('inventories')
        new_inventory_length = len(response.json())
        self.assertTrue(new_inventory_length > old_inventory_length )
    
    
    def test_delete_inventory(self):
        inventory_id = 3
        response = self.client.delete(f'inventories/{inventory_id}')
        self.assertEqual(response.status_code, 200)
        response = self.client.get(f'inventories/{inventory_id}')
        self.assertEqual(response.text, "null")
    
if __name__ == '__main__':
    unittest.main()