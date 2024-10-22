import unittest
from httpx import Client

class TestItemLines(unittest.TestCase): #5 - 7 met post
    def setUp(self):
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_item_lines(self):
        response = self.client.get('item_lines')
        self.assertEqual(response.status_code, 200)
    
    def test_get_single_ItemLine(self):
        transfer_id = 1
        response = self.client.get(f'item_lines/{transfer_id}')
        item_line = response.json()
        self.assertEqual(item_line['id'], transfer_id)
    
    def test_get_nonexistent_item_line(self):
        transfer_id = 1234567
        response = self.client.get(f'item_lines/{transfer_id}')
        self.assertEqual(response.text, "null")

    def test_get_empty_item_line_by_id(self):
        response = self.client.get('item_lines/500000000')
        item_line = response.json()
        self.assertIsNone(item_line)

    def test_get_item_line_invalid_path(self):
        response = self.client.get('/item_lines/1/locations/invalid')
        self.assertEqual(response.status_code, 404)

    # def test_post_item_line(self):
    #     item_line_data = {
    #         "id": 50000001,
    #         "transfer_id": 1,
    #         "item_id": 1,
    #         "quantity": 5,
    #         "created_at": "1983-04-13 04:59:55",
    #         "updated_at": "2007-02-08 20:11:00"
    #     }
    #     post_response = self.client.post('http://localhost:3000/api/v1/item_lines', json=item_line_data)
    #     self.assertEqual(post_response.status_code, 201)
    #     get_response = self.client.get('item_lines/50000001')
    #     item_line = get_response.json()
    #     self.assertEqual(item_line['id'], 50000001)
        # self.assertEqual(item_line['transfer_id'], 1)
    
    # def test_post_item_line_success(self):
    #     response = self.client.get('item_lines')
    #     old_item_line_length = len(response.json())
    #     item_line_data = {
    #         "id": 50000001,
    #         "transfer_id": 1,
    #         "item_id": 1,
    #         "quantity": 5,
    #         "created_at": "1983-04-13 04:59:55",
    #         "updated_at": "2007-02-08 20:11:00"
    #     }
    #     post_response = self.client.post('item_lines', json=item_line_data)
    #     self.assertEqual(post_response.status_code, 201)
    #     new_response = self.client.get('item_lines')
    #     new_response = len(new_response.json())
    #     self.assertTrue(new_response > old_item_line_length)       
    
if __name__ == '__main__':
    unittest.main()