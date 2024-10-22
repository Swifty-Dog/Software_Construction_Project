import unittest
from httpx import Client

class SuppliersTest(unittest.TestCase):
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_suppliers(self):
        # Test to fetch all suppliers
        response = self.client.get('suppliers')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_supplier(self):
        supplier_id = 1  # Ensure this ID exists in your data
        response = self.client.get(f'suppliers/{supplier_id}')
        self.assertEqual(response.status_code, 200)
        supplier = response.json()
        self.assertEqual(supplier['id'], supplier_id)

    def test_get_single_supplier_fail(self):
        # Test fetching a non-existing supplier
        response = self.client.get('suppliers/500000000')
        self.assertEqual(response.status_code, 404)

    def test_post_supplier(self):
        # Test posting a new supplier
        supplier_data = {
            "id": 50000001,  # Ensure this ID is unique
            "name": "Supplier A",
            "contact_info": "contact@supplierA.com",
            "address": "123 Supplier St.",
            "created_at": "2024-10-22T10:00:00",
            "updated_at": "2024-10-22T10:00:00"
        }

        post_response = self.client.post('suppliers', json=supplier_data)
        self.assertEqual(post_response.status_code, 201)

        # Try to get the new supplier
        get_response = self.client.get(f'suppliers/{supplier_data["id"]}')
        self.assertEqual(get_response.status_code, 200)

        returned_supplier = get_response.json()
        self.assertEqual(returned_supplier['id'], supplier_data['id'])
        self.assertEqual(returned_supplier['name'], supplier_data['name'])
        self.assertEqual(returned_supplier['contact_info'], supplier_data['contact_info'])
        self.assertEqual(returned_supplier['address'], supplier_data['address'])

if __name__ == '__main__':
    unittest.main()
