import os,datetime
def get_now():
    rtv=datetime.datetime.utcnow().astimezone()
    if 'GR_DAYS' in os.environ:
        rtv+=datetime.timedelta(days=int(os.environ['GR_DAYS']))
    return rtv