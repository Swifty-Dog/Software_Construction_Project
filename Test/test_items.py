import unittest
from httpx import Client

class Items_Test(unittest.TestCase): #9 
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_get_items(self):
        response = self.client.get('items')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_item(self):
        item_id = "P000001"
        response = self.client.get(f'items/{item_id}')
        item = response.json()
        self.assertEqual(item['uid'], item_id)
    
    def test_get_nonexistent_item(self):
        item_id = 1234567
        response = self.client.get(f'items/{item_id}')
        self.assertEqual(response.text, "null")

    def test_get_empty_item_by_id(self):
        response = self.client.get('items/P0000000001')
        item = response.json()
        self.assertIsNone(item)

    def test_get_item_invalid_path(self):
        response = self.client.get('/items/1/locations/invalid')
        self.assertEqual(response.status_code, 404)

    def test_post_item(self):
        self.base_url = 'http://localhost:3000/api/v1/'
        response = self.client.post(self.base_url + 'items', json={
            "uid": "P000000",
            "code": "sjQ23408K",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        })
        self.assertEqual(response.status_code, 201)
    
    def test_post_item_success(self):
        response = self.client.get('items')
        old_item_length = len(response.json())
        response = self.client.post('items', json={
            "uid": "P000000",
            "code": "sjQ23408K",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        })
        self.assertEqual(response.status_code, 201)
        response = self.client.get('items')
        new_item_length = len(response.json())
        self.assertTrue(new_item_length > old_item_length )

    def test_delete_item(self):
        item_id = "P000045"
        response = self.client.delete(f'items/{item_id}')
        if response.status_code == 200:
            response = self.client.get(f'items/{item_id}')
            self.assertEqual(response.text, "null")

    def test_put_item(self):
        self.base_url = 'http://localhost:3000/api/v1/'
        response = self.client.put(self.base_url + 'items/P000002', json={
            "uid": "P00000P",
            "code": "sjQ23408K",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        })
        self.assertEqual(response.status_code, 200)
        response = self.client.get('items/P00000P')
        item = response.json()
        self.assertEqual(item['uid'], "P00000P")

if __name__ == '__main__':
    unittest.main()