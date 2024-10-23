import unittest
from httpx import Client

class Locations_Test(unittest.TestCase): 
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_get_locations(self):
        response = self.client.get('locations')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_location(self):
        location_id = 2
        response = self.client.get(f'locations/{location_id}')
        location = response.json()
        self.assertEqual(location['id'], location_id)
    
    def test_get_nonexistent_location(self):
        location_id = 1234567
        response = self.client.get(f'locations/{location_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_location_by_id(self):
        response = self.client.get('locations/500000000')
        location = response.json()
        self.assertIsNone(location)

    def test_get_location_invalid_path(self):
        response = self.client.get('/locations/1/locations/invalid')
        self.assertEqual(response.status_code, 404)

    def test_post_location(self):
        location_data = {
            "id": 50000001,
            "warehouse_id": 500000,
            "code": "TEST 1",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('locations', json=location_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('locations/50000001')
        location = get_response.json()
        self.assertEqual(location['id'], 50000001)
        self.assertEqual(location['code'], "TEST 1") 

    def test_post_location_success(self):
        response = self.client.get('locations')
        old_location_length = len(response.json())
        location_data = {
            "id": 50000002,
            "warehouse_id": 500000,
            "code": "TEST 2",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('locations', json=location_data)
        self.assertEqual(post_response.status_code, 201)
        response = self.client.get('locations')
        new_location_length = len(response.json())
        self.assertEqual(new_location_length, old_location_length + 1)

    
    def test_delete_location(self):
        location_id = 50000001
        response = self.client.delete(f'locations/{location_id}')
        if response.status_code == 200:
            response = self.client.get(f'locations/{location_id}')
            self.assertEqual(response.text, "null")

    def test_post_location_length(self):
        response = self.client.get('locations')
        old_location_length = len(response.json())
        location_data = {
            "id": 50000002,
            "warehouse_id": 500000,
            "code": "TEST 2",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('locations', json=location_data)
        self.assertEqual(post_response.status_code, 201)
        response = self.client.get('locations')
        new_location_length = len(response.json())
        self.assertTrue(new_location_length > old_location_length)     

    def test_put_location(self):
        location_id = 3444
        response = self.client.put(f'locations/{location_id}', json={
            "id": 3444,
            "warehouse_id": 500000,
            "code": "TEST 1",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        })
        self.assertEqual(response.status_code, 200)
        response = self.client.get(f'locations/{location_id}')
        location = response.json()
        self.assertEqual(location['code'], "TEST 1")

if __name__ == '__main__':
    unittest.main()