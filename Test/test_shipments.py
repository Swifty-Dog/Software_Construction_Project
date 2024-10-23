import unittest
from httpx import Client

class Shipments_Test(unittest.TestCase): 
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})

    def test_get_shipments(self):
        response = self.client.get('shipments')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_shipment(self):
        shipment_id = 1
        response = self.client.get(f'shipments/{shipment_id}')
        shipment = response.json()
        self.assertEqual(shipment['id'], shipment_id)
    
    def test_get_nonexistent_shipment(self):
        shipment_id = 1234567
        response = self.client.get(f'shipments/{shipment_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_empty_shipment_by_id(self):
        response = self.client.get('shipments/500000000')
        shipment = response.json()
        self.assertIsNone(shipment)
    
    def test_get_shipment_with_invalid_path(self):
        response = self.client.get('/shipments/1/locations/invalid')
        self.assertEqual(response.status_code, 404)
    
    def test_post_shipment(self):
        shipment_data = {
        "id": 98765432,
        "order_id": 1,
        "source_id": 1,
        "order_date": "2021-01-01",
        "request_date": "2021-01-02",
        "shipment_date": "2021-01-03",
        "shipment_type": "I",
        "shipment_status": "Pending",
        "notes": "Test shipment",
        "carrier_code": "DHL",
        "carrier_description": "DHL Express",
        "service_code": "NextDay",
        "payment_type": "Automatic",
        "transfer_mode": "Ground",
        "total_package_count": 1,
        "total_package_weight": 1.0,
        "created_at": "2021-01-01 00:00:00",
        "updated_at": "2021-01-01 00:00:00"
    }
        post_response = self.client.post('shipments', json=shipment_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('shipments/98765432')
        shipment = get_response.json()
        self.assertEqual(shipment['id'], 98765432)
        self.assertEqual(shipment['order_id'], 1)
    
    def test_post_shipment_success_with_length(self):
        response = self.client.get('shipments')
        old_shipment_length = len(response.json())
        shipment_data = {
            "id": 98765433,
            "order_id": 2,
            "source_id": 2,
            "order_date": "2021-01-01",
            "request_date": "2021-01-02",
            "shipment_date": "2021-01-03",
            "shipment_type": "I",
            "shipment_status": "Pending",
            "notes": "Test shipment",
            "carrier_code": "DHL",
            "carrier_description": "DHL Express",
            "service_code": "NextDay",
            "payment_type": "Automatic",
            "transfer_mode": "Ground",
            "total_package_count": 1,
            "total_package_weight": 1.0,
            "created_at": "2021-01-01 00:00:00",
            "updated_at": "2021-01-01 00:00:00"
        }
        post_response = self.client.post('shipments', json=shipment_data)
        self.assertEqual(post_response.status_code, 201)
        response = self.client.get('shipments')
        new_shipment_length = len(response.json())
        self.assertTrue(new_shipment_length > old_shipment_length)
        

    def test_delete_shipment(self):
        shipment_id = 3
        response = self.client.delete(f'shipments/{shipment_id}')
        if response.status_code == 200:
            response = self.client.get(f'shipments/{shipment_id}')
            self.assertEqual(response.text, "null")

    def test_put_shipment(self):
        shipment_id = 1
        shipment_data = {
        "id": 1,
        "order_id": 1391,
        "source_id": 1,
        "order_date": "2021-01-01",
        "request_date": "2021-01-02",
        "shipment_date": "2021-01-03",
        "shipment_type": "I",
        "shipment_status": "Pending",
        "notes": "Test shipment",
        "carrier_code": "DHL",
        "carrier_description": "DHL Express",
        "service_code": "NextDay",
        "payment_type": "Automatic",
        "transfer_mode": "Ground",
        "total_package_count": 1,
        "total_package_weight": 1.0,
        "created_at": "2021-01-01 00:00:00",
        "updated_at": "2021-01-01 00:00:00"
    }
        response = self.client.put(f'shipments/{shipment_id}', json=shipment_data)
        self.assertEqual(response.status_code, 200)
        response = self.client.get(f'shipments/{shipment_id}')
        shipment = response.json()
        self.assertEqual(shipment['id'], shipment_id)
        self.assertEqual(shipment['order_id'], 1391)

    

if __name__ == '__main__':
    unittest.main()

