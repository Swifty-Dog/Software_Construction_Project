import unittest
from httpx import Client

class Orders_Test(unittest.TestCase): # 6 maar 7 met post
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    

    def test_get_orders(self):
        # Test to fetch all orders
        response = self.client.get('orders')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)
    
    def test_get_single_order(self):
        order_id = 1
        response = self.client.get(f'orders/{order_id}')
        order = response.json()
        self.assertEqual(order['id'], order_id)
    
    def test_get_nonexistent_order(self):
        order_id = 98765432
        response = self.client.get(f'orders/{order_id}')
        self.assertEqual(response.text, "null")
    
    def test_get_nonexistent_order_id(self):
        response = self.client.get('orders/500000000')
        order = response.json()
        self.assertIsNone(order)

    def test_get_order_empty_by_id(self):
        response = self.client.get('orders/987654321')
        order = response.json()
        self.assertIsNone(order)
    
    def test_get_orders_with_invalid_path(self):
        response = self.client.get('/orders/1/locations/invalid')
        self.assertEqual(response.status_code, 404)

    def test_post_order(self):
        order_data = {
            "id": 987654,
            "source_id": 52,
            "order_date": "1983-09-26T19:06:08Z",
            "request_date": "1983-09-30T19:06:08Z",
            "reference": "ORD00003",
            "reference_extra": "Vergeven kamer goed enkele wiel tussen.",
            "order_status": "Delivered",
            "notes": "Zeil hoeveel onze map sex ding.",
            "shipping_notes": "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.",
            "picking_notes": "Grof geven politie suiker bodem zuid.",
            "warehouse_id": 11,
            "ship_to": "null",
            "bill_to": "null",
            "shipment_id": 3,
            "total_amount": 1156.14,
            "total_discount": 420.45,
            "total_tax": 677.42,
            "total_surcharge": 86.03,
            "created_at": "1983-09-26T19:06:08Z",
            "updated_at": "1983-09-28T15:06:08Z",
            "items": [
                {
                    "item_id": "P010669",
                    "amount": 16
                }
            ]
        }
        post_response = self.client.post('orders', json=order_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('orders/987654')
        order = get_response.json()
        self.assertEqual(order['id'], 987654)
        self.assertEqual(order['source_id'], 52)

    def test_post_order_success_check_by_length(self):
        response = self.client.get('orders')
        old_order_length = len(response.json())
        response = self.client.post('orders', json= {
            "id": 8989822,
            "source_id": 122323,
            "order_date": "1983-09-26T19:06:08Z",
            "request_date": "1983-09-30T19:06:08Z",
            "reference": "ORD00003",
            "reference_extra": "Vergeven kamer goed enkele wiel tussen.",
            "order_status": "Delivered",
            "notes": "Zeil hoeveel onze map sex ding.",
            "shipping_notes": "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.",
            "picking_notes": "Grof geven politie suiker bodem zuid.",
            "warehouse_id": 11,
            "ship_to": "null",
            "bill_to": "null",
            "shipment_id": 3,
            "total_amount": 1156.14,
            "total_discount": 420.45,
            "total_tax": 677.42,
            "total_surcharge": 86.03,
            "created_at": "1983-09-26T19:06:08Z",
            "updated_at": "1983-09-28T15:06:08Z",
            "items": [
                {
                    "item_id": "P010669",
                    "amount": 16
                }
            ]
        })
        self.assertEqual(response.status_code, 201)
        response = self.client.get('orders')
        new_order_length = len(response.json())
        self.assertTrue(new_order_length > old_order_length)

    def test_delete_order(self):
        order_id = 2
        response = self.client.delete(f'orders/{order_id}')
        if response.status_code == 200:
            response = self.client.get(f'orders/{order_id}')
            self.assertEqual(response.text, "null")

    def test_put_order(self):
        self.base_url = 'http://localhost:3000/api/v1/'
        response = self.client.put(self.base_url + 'orders/444', json={
            "id": 8989822,
            "source_id": 122323,
            "order_date": "1983-09-26T19:06:08Z",
            "request_date": "1983-09-30T19:06:08Z",
            "reference": "TEST",
            "reference_extra": "Vergeven kamer goed enkele wiel tussen.",
            "order_status": "Delivered",
            "notes": "Zeil hoeveel onze map sex ding.",
            "shipping_notes": "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.",
            "picking_notes": "Grof geven politie suiker bodem zuid.",
            "warehouse_id": 11,
            "ship_to": "null",
            "bill_to": "null",
            "shipment_id": 3,
            "total_amount": 1156.14,
            "total_discount": 420.45,
            "total_tax": 677.42,
            "total_surcharge": 86.03,
            "created_at": "1983-09-26T19:06:08Z",
            "updated_at": "1983-09-28T15:06:08Z",
            "items": [
                {
                    "item_id": "P010669",
                    "amount": 16
                }
            ]
        })
        self.assertEqual(response.status_code, 200)
        response = self.client.get('orders/8989822')
        order = response.json()
        self.assertEqual(order['id'], 8989822)
        self.assertEqual(order['reference'], "TEST")
        


if __name__ == '__main__':
    unittest.main()