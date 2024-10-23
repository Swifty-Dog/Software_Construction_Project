import unittest
from httpx import Client

class Suppliers_Test(unittest.TestCase): # 7 
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_supplier_authentication(self):
        self.client_fail = Client(base_url= 'http://localhost:3000/api/v1/')
        response = self.client_fail.get('suppliers')
        self.assertEqual(response.status_code, 401)
    
    def test_get_suppliers(self):
        # Test to fetch all suppliers
        response = self.client.get('suppliers')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_supplier(self):
        supplier_id = 2
        response = self.client.get(f'suppliers/{supplier_id}')
        supplier = response.json()
        self.assertEqual(supplier['id'], supplier_id)
    
    def test_get_nonexistent_supplier(self):
        supplier_id = 1234567
        response = self.client.get(f'suppliers/{supplier_id}')
        self.assertEqual(response.text, "null")

    def test_get_suppliers_with_invalid_path(self):
        response = self.client.get('/suppliers/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    
    def test_post_supplier(self):
        supplier_data = {
            "id": 50000001,
            "code": "TEST",
            "name": "Jason",
            "address": "Karlijndreef 281",
            "address_extra": "Karlijndreef 281",
            "zip_code": "4002 AS",
            "province": "Friesland",
            "country": "NL",
            "contact_name": "Jason",
            "phonenumber": "(078) 0013363",
            "reference": "test",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('suppliers', json=supplier_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('suppliers/50000001')
        return_supplier = get_response.json()
        self.assertEqual(return_supplier['id'], 50000001)
        self.assertEqual(return_supplier['code'], "TEST")
        self.assertEqual(return_supplier['name'], "Jason")
        self.assertEqual(return_supplier['address'], "Karlijndreef 281")
        self.assertEqual(return_supplier['address_extra'], "Karlijndreef 281")
        self.assertEqual(return_supplier['zip_code'], "4002 AS")
        self.assertEqual(return_supplier['province'], "Friesland")
        self.assertEqual(return_supplier['country'], "NL")
    
    def test_post_supplier_success(self):
        response = self.client.get('suppliers')
        old_supplier_length = len(response.json())
        supplier_data = {
            "id": 50000002,
            "code": "TEST",
            "name": "Jason",
            "address": "Karlijndreef 281",
            "address_extra": "Karlijndreef 281",
            "zip_code": "4002 AS",
            "province": "Friesland",
            "country": "NL",
            "contact_name": "Jason",
            "phonenumber": "(078) 0013363",
            "reference": "test",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('suppliers', json=supplier_data)
        self.assertEqual(post_response.status_code, 201)
        response = self.client.get('suppliers')
        new_supplier_length = len(response.json())
        self.assertGreater(new_supplier_length, old_supplier_length)
    
    def test_delete_supplier(self):
        supplier_id = 4000
        response = self.client.delete(f'suppliers/{supplier_id}')
        self.assertEqual(response.status_code, 200)
        response = self.client.get(f'suppliers/{supplier_id}')
        self.assertEqual(response.text, "null")