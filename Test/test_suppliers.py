import unittest
from httpx import Client

class Suppliers_Test(unittest.TestCase):
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_suppliers(self):
        # Test to fetch all suppliers
        response = self.client.get('suppliers')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_supplier(self):   # Test to fetch a specific supplier by its ID
        supplier_id = 1  
        response = self.client.get(f'suppliers/{supplier_id}')
        supplier = response.json()
        self.assertEqual(supplier['id'], supplier_id)

    def test_get_single_supplier_with_False(self):  # Test to fetch a specific supplier by its ID
        supplier_id = 1
        response = self.client.get(f'suppliers/{supplier_id}')
        self.assertEqual(response.status_code, 200)
        supplier = response.json()
        self.assertFalse('invalid_field' in supplier)

    def test_get_single_supplier_fail(self): # Test to fetch a specific supplier that does not exist 
        response = self.client.get('suppliers/500000000')
        self.assertEqual(response.status_code, 404)

    def test_post_supplier(self):
        supplier_data = {
            "id": 50000002,  # Ensure this ID is unique
            "name": "Example Supplier",
            "contact": {
                "name": "John Doe",
                "phone": "(123) 456-7890",
                "email": "johndoe@example.com"
            },
            "address": "123 Supplier St.",
            "city": "Supplier City",
            "province": "Supplier Province",
            "country": "Supplier Country",
            "created_at": "2024-10-22T10:00:00",
            "updated_at": "2024-10-22T10:00:00"
        }

        post_response = self.client.post('suppliers', json=supplier_data)
        self.assertEqual(post_response.status_code, 201)

        get_response = self.client.get(f'suppliers/{supplier_data["id"]}')
        self.assertEqual(get_response.status_code, 200)

        returned_supplier = get_response.json()
        self.assertEqual(returned_supplier['id'], supplier_data['id'])
        self.assertEqual(returned_supplier['name'], supplier_data['name'])
        self.assertEqual(returned_supplier['contact'], supplier_data['contact'])
        self.assertEqual(returned_supplier['address'], supplier_data['address'])
        self.assertEqual(returned_supplier['city'], supplier_data['city'])
        self.assertEqual(returned_supplier['province'], supplier_data['province'])
        self.assertEqual(returned_supplier['country'], supplier_data['country'])

    def test_update_supplier(self):
        supplier_id = 50000002  # Make sure this ID matches the ID from the post test
        updated_data = {
            "name": "Updated Supplier Name",
            "contact": {
                "name": "Jane Doe",
                "phone": "(987) 654-3210",
                "email": "janedoe@example.com"
            },
            "address": "456 Updated St.",
            "city": "Updated City",
            "province": "Updated Province",
            "country": "Updated Country",
            "created_at": "2024-10-22T10:00:00",
            "updated_at": "2024-10-22T10:00:00"
        }

        put_response = self.client.put(f'suppliers/{supplier_id}', json=updated_data)
        self.assertEqual(put_response.status_code, 200)

        get_response = self.client.get(f'suppliers/{supplier_id}')
        self.assertEqual(get_response.status_code, 200)

        returned_supplier = get_response.json()
        self.assertEqual(returned_supplier['id'], supplier_id)
        self.assertEqual(returned_supplier['name'], updated_data['name'])
        self.assertEqual(returned_supplier['contact'], updated_data['contact'])

if __name__ == '__main__':
    unittest.main()
