import unittest
from httpx import Client

class WarehousesTest(unittest.TestCase):
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_warehouses(self):
        # Test to fetch all warehouses
        response = self.client.get('warehouses')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)  

    def test_get_single_warehouse(self):   # Test to fetch a specific warehouse by its ID )
        warehouse_id = 1  
        response = self.client.get(f'warehouses/{warehouse_id}')
        warehouse = response.json()
        # Check if the returned 'id' matches the requested warehouse ID
        self.assertEqual(warehouse['id'], warehouse_id)

    def test_get_single_warehouse_with_False(self):
        # Test to fetch a specific warehouse by its ID
        warehouse_id = 1
        response = self.client.get(f'warehouses/{warehouse_id}')
        self.assertEqual(response.status_code, 200)
        warehouse = response.json()
        # Ensure that the warehouse ID matches and there are no unexpected fields
        self.assertFalse('invalid_field' in warehouse)

    def test_get_single_warehouse_fail(self): # Test to fetch a specific warehouse that does not exist 
        response = self.client.get('warehouses/500000000')
        warehouse = response.json()
        # Check if the warehouse is None or if it's an empty dictionary
        self.assertIsNone(warehouse)

    def test_get_warehouses_fail(self):  # Test to fetch all warehouses
        response = self.client.get('/warehouses/1/locations/invalid_segment') # wrong get request 
        self.assertEqual(response.status_code, 404)

    def test_get_locations_in_warehouse(self):
        # Test fetching all locations for a specific warehouse
        response = self.client.get('warehouses/1/locations')
        self.assertEqual(response.status_code, 200)  # check for status code
        self.assertGreater(len(response.json()), 0)  # Ensure locations are returned

    def test_post_warehouse(self):
        warehouse_data = {
            "id": 9999999999999,
            "code": "YQZZNL56",
            "name": "Heemskerk cargo hub",
            "address": "Karlijndreef 281",
            "zip": "4002 AS",
            "city": "Heemskerk",
            "province": "Friesland",
            "country": "NL",
            "contact": {
                "name": "null",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }

        response = self.client.post('warehouses', json=warehouse_data)
        self.assertEqual(response.status_code, 201)

if __name__ == '__main__':
    unittest.main()
