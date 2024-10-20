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

    def test_get_single_warehouse(self):
        # Test to fetch a specific warehouse
        response = self.client.get('warehouses/1')
        self.assertEqual(response.status_code, 200)
        warehouse = response.json()
        self.assertIsInstance(warehouse, dict)  # Ensure response is a dictionary (single warehouse)
        self.assertEqual(warehouse['id'], 1)  # Ensure the correct warehouse is fetched

    # def test_get_invalid_warehouse(self):
    #     # Test fetching a warehouse with an invalid ID
    #     response = self.client.get('warehouses/-1')
    #     self.assertEqual(response.status_code, 404)

    def test_get_locations_in_warehouse(self):
        # Test fetching all locations for a specific warehouse
        response = self.client.get('warehouses/1/locations')
        self.assertEqual(response.status_code, 200)  # check for status code
        self.assertGreater(len(response.json()), 0)  # Ensure locations are returned

    # def test_get_invalid_warehouse_locations(self):
    #     # Test fetching locations for an invalid warehouse ID
    #     response = self.client.get('warehouses/-1/locations')
    #     self.assertEqual(response.status_code, 404)

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
        # if response.content:
        #     response_data = response.json()
        #     self.assertEqual(response_data['id'], warehouse_data['id'])
        # else:
        #     self.fail("No content in the response")
  

if __name__ == '__main__':
    unittest.main()
