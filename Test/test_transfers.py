import unittest
from httpx import Client

class Transfers_Test(unittest.TestCase):
    def setUp(self):  
        API_KEY = "a1b2c3d4e5"
        self.client = Client(base_url='http://localhost:3000/api/v1/', headers={"API_KEY": API_KEY})
    
    def test_get_Transfers(self):
        # Test to fetch all transfers
        response = self.client.get('transfers')
        self.assertEqual(response.status_code, 200)
        self.assertGreater(len(response.json()), 0)

    def test_get_single_transfer(self):
        transfer_id = 1
        response = self.client.get(f'transfers/{transfer_id}')
        transfer = response.json()
        self.assertEqual(transfer['id'], transfer_id)
    
    def test_get_single_transfer_with_False(self):
        transfer_id = 1
        response = self.client.get(f'transfers/{transfer_id}')
        self.assertEqual(response.status_code, 200)
        transfer = response.json()
        self.assertFalse('invalid_field' in transfer)
    
    def test_get_single_transfer_fail(self):
        response = self.client.get('transfers/500000000')
        transfer = response.json()
        self.assertIsNone(transfer)
    
    def test_get_transfers_fail(self):
        response = self.client.get('/transfers/1/locations/invalid')
        self.assertEqual(response.status_code, 404)

    def test_commit_transfer(self): 
        transfer_id = 1
        response = self.client.put(f'transfers/{transfer_id}/commit')
        ## deze moet nog gemaakt worden api voor commit werkt nu niet 


    def test_post_transfer(self):
        transfer_data = {
            "id": 50000001,
            "from_warehouse": 1,
            "to_warehouse": 2,
            "status": "pending",
            "created_at": "1983-04-13 04:59:55",
            "updated_at": "2007-02-08 20:11:00"
        }
        post_response = self.client.post('transfers', json=transfer_data)
        self.assertEqual(post_response.status_code, 201)
        get_response = self.client.get('transfers/50000001')
        transfer = get_response.json()
        self.assertEqual(transfer['id'], 50000001)
        self.assertEqual(transfer['from_warehouse'], 1)
        

if __name__ == '__main__':
    unittest.main()
