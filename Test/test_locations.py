import unittest
from httpx import Client

class LocationsTest(unittest.TestCase):
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_locations(self):
        # Test to fetch all locations
        response = self.client.get('locations')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_location(self):
        location_id = 1  # Ensure this ID exists in your data
        response = self.client.get(f'locations/{location_id}')
        self.assertEqual(response.status_code, 200)
        location = response.json()
        self.assertEqual(location['id'], location_id)

    def test_get_single_location_fail(self):
        response = self.client.get('locations/500000000')
        self.assertEqual(response.status_code, 404)

    def test_post_location(self):
        location_data = {
            "id": 50000010,  # Ensure this ID is unique
            "warehouse_id": 1,
            "code": "LOC001",
            "name": "Main Warehouse Location",
            "description": "Primary storage area",
            "created_at": "2024-10-22T10:00:00",
            "updated_at": "2024-10-22T10:00:00"
        }

        post_response = self.client.post('locations', json=location_data)
        self.assertEqual(post_response.status_code, 201)

        get_response = self.client.get(f'locations/{location_data["id"]}')
        self.assertEqual(get_response.status_code, 200)

        returned_location = get_response.json()
        self.assertEqual(returned_location['id'], location_data['id'])
        self.assertEqual(returned_location['warehouse_id'], location_data['warehouse_id'])
        self.assertEqual(returned_location['code'], location_data['code'])
        self.assertEqual(returned_location['name'], location_data['name'])
        self.assertEqual(returned_location['description'], location_data['description'])

if __name__ == '__main__':
    unittest.main()
