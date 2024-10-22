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
        self.assertGreater(len(response.json()), 0)  # Ensure at least one supplier exists

    def test_post_supplier(self):
        # Test posting a new supplier
        supplier_data = {
            "id": 50000001,
            "code": "SUP0005",
            "name": "New Supplier Inc.",
            "address": "123 New Address",
            "address_extra": "Suite 1A",
            "city": "New City",
            "zip_code": "12345",
            "province": "New Province",
            "country": "New Country",
            "contact_name": "Alice Smith",
            "phonenumber": "123-456-7890",
            "reference": "NS-SUP0005",
            "created_at": "2024-10-22T10:00:00",
            "updated_at": "2024-10-22T10:00:00"
        }

        post_response = self.client.post('suppliers', json=supplier_data)
        self.assertEqual(post_response.status_code, 201)

        # Verify that the supplier was added
        get_response = self.client.get(f'suppliers/{supplier_data["id"]}')
        self.assertEqual(get_response.status_code, 200)

        returned_supplier = get_response.json()
        self.assertEqual(returned_supplier['id'], supplier_data['id'])
        self.assertEqual(returned_supplier['code'], supplier_data['code'])
        self.assertEqual(returned_supplier['name'], supplier_data['name'])
        self.assertEqual(returned_supplier['address'], supplier_data['address'])
        self.assertEqual(returned_supplier['address_extra'], supplier_data['address_extra'])
        self.assertEqual(returned_supplier['city'], supplier_data['city'])
        self.assertEqual(returned_supplier['zip_code'], supplier_data['zip_code'])
        self.assertEqual(returned_supplier['province'], supplier_data['province'])
        self.assertEqual(returned_supplier['country'], supplier_data['country'])
        self.assertEqual(returned_supplier['contact_name'], supplier_data['contact_name'])
        self.assertEqual(returned_supplier['phonenumber'], supplier_data['phonenumber'])
        self.assertEqual(returned_supplier['reference'], supplier_data['reference'])

if __name__ == '__main__':
    unittest.main()
