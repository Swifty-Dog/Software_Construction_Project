import unittest
from httpx import Client

class Clients_Test(unittest.TestCase): # 6 maar 7 met post
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_get_clients(self):
        # Test to fetch all clients
        response = self.client.get('clients')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_client(self):
        client_id = 1
        response = self.client.get(f'clients/{client_id}')
        client = response.json()
        self.assertEqual(client['id'], client_id)
    
    def test_get_nonexistent_client(self):
        client_id = 1234567
        response = self.client.get(f'clients/{client_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_client_by_id(self):
        response = self.client.get('clients/500000000')
        client = response.json()
        self.assertIsNone(client)
    
    def test_get_client_invalid_path(self):
        response = self.client.get('/clients/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    
    def test_post_client(self):
        self.base_url = 'http://localhost:3000/api/v1/'
        response = self.client.post(self.base_url + "clients", json={
            "id": 1,
            "name": "Raymond Inc",
            "address": "1296 Daniel Road Apt. 349",
            "city": "Pierceview",
            "zip_code": "28301",
            "province": "Colorado",
            "country": "United States",
            "contact_name": "Bryan Clark",
            "contact_phone": "242.732.3483x2573",
            "contact_email": "test@",
            "created_at": "2010-04-28 02:22:53",
            "updated_at": "2022-02-09 20:22:35"
        })
        self.assertEqual(response.status_code, 201)
    
    def test_post_client_success_with_length(self):
        response = self.client.get('clients')
        old_client_length = len(response.json())
        response = self.client.post('clients', json={
            "id": 1,
            "name": "Raymond Inc",
            "address": "1296 Daniel Road Apt. 349",
            "city": "Pierceview",
            "zip_code": "28301",
            "province": "Colorado",
            "country": "United States",
            "contact_name": "Bryan Clark",
            "contact_phone": "242.732.3483x2573",
            "contact_email": "test@",
            "created_at": "2010-04-28 02:22:53",
            "updated_at": "2022-02-09 20:22:35"
        })
        self.assertEqual(response.status_code, 201)
        response = self.client.get('clients')
        new_client_length = len(response.json())
        self.assertTrue(new_client_length > old_client_length)
       

if __name__ == '__main__':
    unittest.main()

