import unittest
from httpx import Client

class clients_Test(unittest.TestCase): 
    def setUp(self):
        self.reader_token = "f6g7h8i9j0"
        self.client = Client(base_url="http://localhost:3000/api/v1/",
                             headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})
 
    def test_get_all_readertoken(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/clients")
 
        self.assertEqual(response.status_code, 200)
   
    def test_get_single_readertoken(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/clients/8")
 
        self.assertEqual(response.status_code, 200)
   
    def test_unauthorized_get_all(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/clients")
 
        self.assertEqual(response.status_code, 401)
 
    def test_get_single_incorrect_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/clients/-90")
 
        self.assertEqual(response.status_code, 401)
 
    def test_get_single_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/clients/8")
 
        self.assertEqual(response.status_code, 401)
 
    def test_create_client_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        new_client = {
            "id": 1,
            "name": "John Doe",
            "address": "Wijnhaven 500",
            "city": "Rotterdam",
            "zip_code": "3000",
            "province": "Zuid-Holland",
            "country": "Netherlands",
            "contact_name": "John",
            "contact_phone": "0611111111",
            "contact_email": "test@gmail.com",
            "created_at": "2024-01-01 00:00:00",
            "updated_at": "2024-01-01 00:00:01"
 
        }
 
        response = self.client.post("/clients", json=new_client)
        self.assertEqual(response.status_code, 401)
 
    def test_update_client_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        updated_client = {
            "id": 1,
            "name": "John does",
            "address": "Wijnhaven 555",
            "city": "Amsterdam",
            "zip_code": "3000",
            "province": "Zuid-Holland",
            "country": "Netherlands",
            "contact_name": "John",
            "contact_phone": "0622222222",
            "contact_email": "test2@gmail.com",
            "created_at": "2024-01-01 00:00:00",
            "updated_at": "2024-01-01 00:00:05"
        }
 
        response = self.client.put("/clients/3", json=updated_client)
 
        self.assertEqual(response.status_code, 401)
 
    def test_delete_client_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.delete("/clients/90")
 
        self.assertEqual(response.status_code, 401)
 
    def test_delete_incorrect_id_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.delete("/clients/-10")
 
        self.assertEqual(response.status_code, 401)
 
    def test_get_all(self):
        response = self.client.get("/clients")
 
        self.assertEqual(response.status_code, 200)
        self.assertTrue(len(response.json()) > 0)
 
    def test_get_single(self):
        response = self.client.get("/clients/8")
 
        self.assertEqual(response.status_code, 200)
        self.assertIn("id", response.json())
 
    def test_get_incorrect_id(self):
        response = self.client.get("/clients/-1")
 
        self.assertEqual(response.status_code, 404)
 
    def test_create_client(self):
        new_client = {
            "id": 15,
            "name": "Test Doe",
            "address": "Wijnhaven  107",
            "city": "Rotterdam",
            "zip_code": "idk",
            "province": "Zuid-Holland",
            "country": "NL",
            "contact_name": "Thomas",
            "contact_phone": "0612345678",
            "contact_email": "Thomas@gmail.com",
            "created_at": "2000-01-01 00:00:00",
            "updated_at": "2020-01-01 00:00:00"
        }

        response = self.client.post("/clients", json=new_client)
        self.assertEqual(response.status_code, 201)
 
    def test_update_client(self):
        updated_client = {
            "id": 15,
            "name": "Test Test",
            "address": "Wijnhaven  999",
            "city": "Amsterdam",
            "zip_code": "00000000",
            "province": "Noord-Holland",
            "country": "NL",
            "contact_name": "Tim",
            "contact_phone": "0699999999",
            "contact_email": "Tim@gmail.com",
            "created_at": "2000-01-01 00:00:00",
            "updated_at": "2020-01-01 00:00:00"
        }
 
        response = self.client.put("/clients/3", json=updated_client)
 
        self.assertEqual(response.status_code, 200)
 
    def test_delete_client(self):
        response = self.client.delete("/clients/90")
 
        self.assertEqual(response.status_code, 200)
 
    def test_delete_incorrect_id(self):
        response = self.client.delete("/clients/-10")
 
        self.assertEqual(response.status_code, 404)


if __name__ == '__main__':
    unittest.main()

