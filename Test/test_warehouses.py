import unittest
from httpx import Client

class Warehouses_Test(unittest.TestCase):
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
        self.assertEqual(warehouse['id'], warehouse_id)

    def test_get_single_warehouse_with_False(self):  # Test to fetch a specific warehouse by its ID
        warehouse_id = 1
        response = self.client.get(f'warehouses/{warehouse_id}')
        self.assertEqual(response.status_code, 200)
        warehouse = response.json()
        self.assertFalse('invalid_field' in warehouse)

    def test_get_single_warehouse_fail(self): # Test to fetch a specific warehouse that does not exist 
        response = self.client.get('warehouses/500000000')
        warehouse = response.json()
        self.assertIsNone(warehouse)

    def test_get_warehouses_fail(self):  # Test to fetch all warehouses
        response = self.client.get('/warehouses/1/locations/invalid_segment') # wrong get request 
        self.assertEqual(response.status_code, 404)

    def test_get_locations_in_warehouse(self): # Test fetching all locations for a specific warehouse
        response = self.client.get('warehouses/1/locations')
        self.assertEqual(response.status_code, 200)  
        self.assertGreater(len(response.json()), 0)  

    def test_post_warehouse(self):
        warehouse_data = {
            "id": 50000001,
            "code": "YQZZNL56",
            "name": "Heemskerk cargo hub",
            "address": "Karlijndreef 281",
            "zip": "4002 AS",
            "city": "Heemskerk",
            "province": "Friesland",
            "country": "NL",
            "contact": {
                "name": "Jason",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }

        # Post the new warehouse
        post_response = self.client.post('warehouses', json=warehouse_data)
        self.assertEqual(post_response.status_code, 201)

        get_response = self.client.get(f'warehouses/{warehouse_data["id"]}')
        self.assertEqual(get_response.status_code, 200)

        returned_warehouse = get_response.json()
        self.assertEqual(returned_warehouse['id'], warehouse_data['id'])
        self.assertEqual(returned_warehouse['code'], warehouse_data['code'])
        self.assertEqual(returned_warehouse['name'], warehouse_data['name'])
        self.assertEqual(returned_warehouse['address'], warehouse_data['address'])
        self.assertEqual(returned_warehouse['zip'], warehouse_data['zip'])
        self.assertEqual(returned_warehouse['city'], warehouse_data['city'])
        self.assertEqual(returned_warehouse['province'], warehouse_data['province'])
        self.assertEqual(returned_warehouse['country'], warehouse_data['country'])
        self.assertEqual(returned_warehouse['contact'], warehouse_data['contact'])
        # self.assertEqual(returned_warehouse['created_at'], warehouse_data['created_at'])
        # self.assertEqual(returned_warehouse['updated_at'], warehouse_data['updated_at'])


if __name__ == '__main__':
    unittest.main()
