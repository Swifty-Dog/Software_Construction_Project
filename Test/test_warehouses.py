import unittest
from httpx import Client

class Warehouses_Test(unittest.TestCase): # 6 maar 7 met post 
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_warehouse_authentication(self):
        self.client_fail = Client(base_url= 'http://localhost:3000/api/v1/')
        response = self.client_fail.get('warehouses')
        self.assertEqual(response.status_code, 401)
 

    def test_get_warehouses(self):
        # Test to fetch all warehouses
        response = self.client.get('warehouses')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)  

    def test_get_single_warehouse(self):   # Test to fetch a specific warehouse by its ID )
        warehouse_id = 2
        response = self.client.get(f'warehouses/{warehouse_id}')
        warehouse = response.json()
        self.assertEqual(warehouse['id'], warehouse_id)


    def test_get_nonexistent_warehouse(self): 
        warehouse_id = 1234567
        response = self.client.get(f'warehouses/{warehouse_id}')
        self.assertEqual(response.text, "null")  #the response body is empty

    def test_get_warehouses_invalid_path(self):  # Test to fetch warehouses with an invalid URL segment
        response = self.client.get('/warehouses/1/locations/invalid_segment')  # wrong GET request
        self.assertEqual(response.status_code, 404)

    def test_get_locations_in_warehouse(self): # Test fetching all locations for a specific warehouse
        response = self.client.get('warehouses/1/locations')
        self.assertEqual(response.status_code, 200)  
        self.assertGreater(len(response.json()), 0)  

    def test_delete_warehouse(self):
        warehouse_id = 50000001
        response = self.client.delete(f'warehouses/{warehouse_id}')
        # self.assertEqual(response.status_code, 200)
        if response.status_code == 200:
            response = self.client.get(f'warehouses/{warehouse_id}')
            self.assertEqual(response.text, "null")
    
    # def test_post_warehouse(self):
    #     warehouse_data = {
    #         "id": 1234567,
    #         "code": "TEST",
    #         "name": "Heemskerk cargo hub",
    #         "address": "Karlijndreef 281",
    #         "zip": "4002 AS",
    #         "city": "Heemskerk",
    #         "province": "Friesland",
    #         "country": "NL",
    #         "contact": {
    #             "name": "Jason",
    #             "phone": "(078) 0013363",
    #             "email": "blamore@example.net"
    #         },
    #         "created_at": "1983-04-13 04:59:55",
    #         "updated_at": "2007-02-08 20:11:00"
    #     }

    #     # Post the new warehouse
    #     post_response = self.client.post('http://localhost:3000/api/v1/warehouses', json=warehouse_data)
    #     self.assertEqual(post_response.status_code, 201)
    #     get_response = self.client.get(f'http://localhost:3000/api/v1/warehouses/1234567/')
    #     self.assertEqual(get_response.status_code, 200)
    #     returned_warehouse = get_response.json()
    #     self.assertEqual(returned_warehouse['id'], warehouse_data['id'])
    #     self.assertEqual(returned_warehouse['code'], warehouse_data['code'])
    #     self.assertEqual(returned_warehouse['name'], warehouse_data['name'])
    #     self.assertEqual(returned_warehouse['address'], warehouse_data['address'])
    #     self.assertEqual(returned_warehouse['zip'], warehouse_data['zip'])
    #     self.assertEqual(returned_warehouse['city'], warehouse_data['city'])
    #     self.assertEqual(returned_warehouse['province'], warehouse_data['province'])
    #     self.assertEqual(returned_warehouse['country'], warehouse_data['country'])
    #     self.assertEqual(returned_warehouse['contact'], warehouse_data['contact'])

    def test_post_warehouse_succesful(self):
        response = self.client.get('warehouses')
        OldLength = len(response.json())

        
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
                "email": "rotterdam@ger",
            },
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        self.client.post('warehouses', json=warehouse_data)
        new_response = self.client.get('warehouses')
        NewLength = len(new_response.json())

        self.assertTrue(NewLength > OldLength)

if __name__ == '__main__':
    unittest.main()
